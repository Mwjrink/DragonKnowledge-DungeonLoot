using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

using DKDG.Utils;

namespace DKDG.Models
{
    public class Database
    {
        #region Fields

        private Dictionary<int, bool> usedIds = new Dictionary<int, bool>();
        private bool usedIdsFull = false;
        private readonly object idLock = new object();

        private readonly string path = "";

        //private readonly Dictionary<long, Armor> loadedArmor = new Dictionary<long, Armor>();
        //private readonly Dictionary<long, Background> loadedBackground = new Dictionary<long, Background>();
        //private readonly Dictionary<long, Character> loadedCharacter = new Dictionary<long, Character>();
        //private readonly Dictionary<long, Class> loadedClass = new Dictionary<long, Class>();
        //private readonly Dictionary<long, Language> loadedLanguage = new Dictionary<long, Language>();
        //private readonly Dictionary<long, Monster> loadedMonster = new Dictionary<long, Monster>();
        //private readonly Dictionary<long, NPC> loadedNPC = new Dictionary<long, NPC>();
        //private readonly Dictionary<long, OtherEquipment> loadedOtherEquipment = new Dictionary<long, OtherEquipment>();
        //private readonly Dictionary<long, Race> loadedRace = new Dictionary<long, Race>();
        //private readonly Dictionary<long, Skill> loadedSkill = new Dictionary<long, Skill>();
        //private readonly Dictionary<long, Spell> loadedSpells = new Dictionary<long, Spell>();
        //private readonly Dictionary<long, Tool> loadedTool = new Dictionary<long, Tool>();
        //private readonly Dictionary<long, Weapon> loadedWeapon = new Dictionary<long, Weapon>();
        public static readonly long DEFAULT_ID = -1;

        #endregion Fields

        #region Constructors

        public Database(string path)
        {
            this.path = path;
        }

        #endregion Constructors

        #region Methods

        public bool InitializeDb()
        {
            /*
            sqlite> PRAGMA foreign_keys;
            0
            sqlite> PRAGMA foreign_keys = ON;
            sqlite> PRAGMA foreign_keys;
            1
            */

            //turn on fkeys
            //get all types
            //order them
            //gen tables

            foreach (Type v in OrderTypes(Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsClass && t.GetInterfaces().Contains(typeof(ISavable)))))
                GenerateTables(v);

            return true;
        }

        public int NextId()
        {
            int index = 0;

            lock (idLock)
                if (usedIdsFull)
                    usedIds.Add(index = usedIds.Max(x => x.Key), true);
                else
                    while (true)
                        if (usedIds.TryGetValue(index, out bool used))
                            if (!used)
                            {
                                usedIds.Add(index, true);
                                if (index == usedIds.Max(x => x.Key))
                                    usedIdsFull = true;
                                break;
                            }
                            else
                                index++;

            return index;
        }

        private void GenericFillDict<T>()
        {
            //get dict to load into
        }

        private const string UNIQUE_CONSTRAINT = "UNIQUE";
        private const string NOT_NULL_CONSTRAINT = "NOT NULL";

        /// <summary>
        /// USE STRING.FORMAT, the parameter is the value itself
        /// </summary>
        private const string DEFAULT_VALUE = "DEFAULT {0}";

        /// <summary>
        /// USE STRING.FORMAT, first parameter is the table it refers to, this assumes use of ID column as FKey
        /// </summary>
        private const string FOREIGN_KEY_CONSTRAINT = "REFERENCES {1} (ID)"; //ON DELETE CASCADE ON UPDATE NO CASCADE

        private const string ON_UPDATE = "ON UPDATE";
        private const string ON_DELETE = "ON DELETE";

        private const string NO_ACTION = "NO ACTION";
        private const string RESTRICT = "RESTRICT";
        private const string SET_NULL = "SET NULL";
        private const string SET_DEFAULT = "SET DEFAULT";
        private const string CASCADE = "CASCADE";
        //COMMA AT THE END OF EACH STATEMENT/LINE/COLUMN

        /*
NULL. The value is a NULL value.

INTEGER. The value is a signed integer, stored in 1, 2, 3, 4, 6, or 8 bytes depending on the magnitude of the value.

REAL. The value is a floating point value, stored as an 8-byte IEEE floating point number.

TEXT. The value is a text string, stored using the database encoding (UTF-8, UTF-16BE or UTF-16LE).

BLOB The value is a blob of data, stored exactly as it was input.
         */

        /*
        contact_id integer PRIMARY KEY,
        first_name text NOT NULL,
        last_name text NOT NULL,
        email text NOT NULL UNIQUE,
        phone text NOT NULL UNIQUE

         CREATE TABLE contact_groups (
         contact_id integer,
         group_id integer,
         PRIMARY KEY (contact_id, group_id),
         FOREIGN KEY (contact_id) REFERENCES contacts (contact_id)
         ON DELETE CASCADE ON UPDATE NO ACTION,
         FOREIGN KEY (group_id) REFERENCES groups (group_id)
         ON DELETE CASCADE ON UPDATE NO ACTION
        );
        */

        //IMAGES TABLE WITH AN ID COLUMN AND AN IMAGES COLUMN

        //CREATE TABLE track(
        //trackid INTEGER,
        //trackname TEXT,
        //trackartist INTEGER DEFAULT 0 REFERENCES artist(artistid) ON DELETE SET DEFAULT
        //);

        private List<Type> OrderTypes(IEnumerable<Type> savables, HashSet<string> tested = null, List<(Type child, Type parent)> relationships = null)
        {
            if (tested == null)
                tested = new HashSet<string>();

            if (relationships == null)
                relationships = new List<(Type child, Type parent)>();

            foreach (Type savable in savables)
            {
                if (tested.Add(savable.Name))
                {
                    IEnumerable<PropertyInfo> properties = savable.GetType().GetProperties()
                                                            .Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

                    foreach (PropertyInfo prop in properties)
                    {
                        if (prop.Name == "ID")
                            continue;

                        var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();

                        switch (attribute.SaveRelationship)
                        {
                            case SQLPropSaveType.MultipleParent:
                            case SQLPropSaveType.Link:
                            case SQLPropSaveType.MultipleChild:
                                Type operable = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                                    ? prop.PropertyType.GenericTypeArguments.FirstOrDefault()
                                    : prop.PropertyType;

                                if (operable == null)
                                    throw new Exception("An IEnumerable type was requested to be saved to the Db without a recognizable child type. Not quite sure how this happened");

                                if (operable.GetInterfaces().Any(x => x == typeof(ISavable)))
                                    throw new Exception("Cannot declare a child relationship on an object that is not ISavable");

                                relationships.Add((prop.PropertyType, savable));
                                OrderTypes(new[] { prop.PropertyType }, tested);
                                break;
                        }
                    }
                }
            }

            var topMostParents = new List<Type>();

            foreach (Type savable in savables)
            {
                bool add = true;
                foreach ((Type child, Type parent) in relationships)
                    if (child == savable)
                    {
                        add = false;
                        break;
                    }

                if (add)
                    topMostParents.Add(savable);
            }

            return topMostParents;
        }

        private void GenerateTables(Type savable, HashSet<string> generated = null, PropertyInfo propSaving = null)
        {
            if (generated == null)
                generated = new HashSet<string>();

            var mps = new List<PropertyInfo>();
            var mcs = new List<PropertyInfo>();
            var lks = new List<PropertyInfo>();
            var vls = new List<PropertyInfo>();

            if (generated.Add(savable.Name))
            {
                IEnumerable<PropertyInfo> properties = savable.GetType().GetProperties()
                                                        .Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

                int i = 0;
                string query = "";
                var parameters = new List<(string, DbType, int, object)>();

                query += "CREATE TABLE [IF NOT EXISTS] @" + i + "(";
                parameters.Add(("@" + i, DbType.String, -1, savable.GetType().Name));

                //TODO Max, make sure this gives "ID" not "ISavable.ID"
                query += "@" + ++i;
                parameters.Add(("@" + i, DbType.String, -1, nameof(ISavable.ID)));

                //Generate in the following order
                //multiParent
                //this & value
                //multichild
                foreach (PropertyInfo prop in properties)
                {
                    if (prop.Name == "ID")
                        continue;

                    var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
                    query += "@" + ++i + " PRIMARY KEY";

                    switch (attribute.SaveRelationship)
                    {
                        case SQLPropSaveType.MultipleParent:
                        {
                            mps.Add(prop);

                            string data = String.Join(" ", prop.Name, !attribute.Nullable ? NOT_NULL_CONSTRAINT : "", attribute.Unique ? UNIQUE_CONSTRAINT : "",
                                    attribute.DefaultValue != null ? String.Format(DEFAULT_VALUE, attribute.DefaultValue) : "",
                                    String.Format(FOREIGN_KEY_CONSTRAINT, prop.PropertyType.Name), ON_DELETE, CASCADE, ON_UPDATE, CASCADE);
                            //if(attribute.OnDelete == FKeyActions.Cascade)

                            parameters.Add(("@" + i, DbType.String, -1, data));
                        }
                        break;

                        case SQLPropSaveType.Link:
                            lks.Add(prop);
                            break;

                        case SQLPropSaveType.MultipleChild:
                            mcs.Add(prop);
                            break;

                        case SQLPropSaveType.Value:
                        {
                            vls.Add(prop);
                            if (prop.PropertyType.GetInterfaces().Any(x => x == typeof(ISavable)))
                                throw new InvalidOperationException(String.Format("You are using a value relationship in order to save {0} under the name: {1} " +
                                    "under an object of type {2} when it is an ISavable, use multiparent for this", prop.PropertyType, prop.Name, savable.Name));

                            string data = String.Join(" ", prop.Name, !attribute.Nullable ? NOT_NULL_CONSTRAINT : "", attribute.Unique ? UNIQUE_CONSTRAINT : "",
                                    attribute.DefaultValue != null ? String.Format(DEFAULT_VALUE, attribute.DefaultValue) : "");

                            parameters.Add(("@" + i, DbType.String, -1, prop.Name));
                        }
                        break;
                    }
                }

                query += ");";

                mps.ForEach(x => GenerateTables(GetUseableTypeFromProp(x), generated));

                SQLHelper.ExecuteNonQuery(query, parameters, path);

                lks.ForEach(x => GenerateTables(GetUseableTypeFromProp(x), generated));

                i = 0;
                parameters.Clear();
                foreach (PropertyInfo lk in lks)
                {
                    query = "CREATE TABLE [IF NOT EXISTS] " + lk.PropertyType.Name;

                    query += "( " + Environment.NewLine;

                    query += String.Format("{0} INTEGER {1} REFERENCES {2}(ID) {3} {4} {5} {6}",
                        lk.DeclaringType.Name, NOT_NULL_CONSTRAINT, lk.DeclaringType.Name, ON_UPDATE,
                        CASCADE, ON_DELETE, CASCADE);

                    query += ", " + Environment.NewLine;

                    if (lk.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault() is SQLPropAttribute att && att.CustomColumnName != null)
                        query += String.Join(" ", att.CustomColumnName, !att.CustomPropAttributes.Nullable ? NOT_NULL_CONSTRAINT : "", att.CustomPropAttributes.Unique ? UNIQUE_CONSTRAINT : "",
                            att.CustomPropAttributes.DefaultValue != null ? String.Format(DEFAULT_VALUE, att.CustomPropAttributes.DefaultValue) : "");

                    query += ", " + Environment.NewLine;

                    query += String.Format("{0} INTEGER {1} REFERENCES {2}(ID) {3} {4} {5} {6}",
                        lk.PropertyType.Name, NOT_NULL_CONSTRAINT, lk.PropertyType.Name, ON_UPDATE,
                        CASCADE, ON_DELETE, CASCADE);

                    query += ");";
                }

                //gen the link tables
                //generated.Add(lks)

                mcs.ForEach(x => GenerateTables(GetUseableTypeFromProp(x), generated, x));
            }
        }

        public ISavable Load<T>(long ID)
            where T : ISavable, new()
        {
            ////get properties in class and load
            T loaded = Activator.CreateInstance<T>();

            //IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

            //var aex = new List<(ISavable, Exception)>();

            //var saveDict = new Dictionary<SQLPropSaveType, List<(PropertyInfo, SQLPropAttribute)>>() {
            //        { SQLPropSaveType.Link, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
            //        { SQLPropSaveType.MultipleChild, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
            //        { SQLPropSaveType.MultipleParent, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
            //        { SQLPropSaveType.Value, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() }
            //    };

            //foreach (PropertyInfo prop in properties)
            //{
            //    var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
            //    saveDict[attribute.SaveRelationship].Add((prop, attribute));
            //}

            //foreach (T item in items)
            //{
            //    //Save in the following order:
            //    //multiParents
            //    //Self with values and multiparentids inserted
            //    //multiChilds
            //    //link
            //    CheckInsert(item);
            //    try
            //    {
            //        foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> savableGrouping in saveDict[SQLPropSaveType.MultipleParent].GroupBy(x => x.Item1.PropertyType))
            //            Save(savableGrouping.Select(x => x.Item1.GetValue(item) as ISavable ??
            //            throw new ArgumentException("Cannot declare a MULTI-PARENT relationship with an object that is not ISavable.")),
            //            item.ID, SQLPropSaveType.MultipleParent);

            //        {
            //            string query = "INSERT INTO @TableName ";
            //            string columnNames = "(";
            //            string values = "(";
            //            var parameters = new List<(string, DbType, int, object)>();
            //            int i = 0;

            //            foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.Value])
            //            {
            //                parameters.Add(("@" + ++i, DbType.String, -1, prop.Name));
            //                columnNames += "@" + i + ", ";

            //                object value = prop.GetValue(item);
            //                parameters.Add(("@" + ++i, toDbType(att.SaveType), -1, value));
            //                values += "@" + i + ", ";
            //            }

            //            foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.MultipleParent])
            //            {
            //                parameters.Add(("@" + ++i, DbType.String, -1, prop.Name));
            //                columnNames += "@" + i + ", ";

            //                var value = prop.GetValue(item) as ISavable;
            //                parameters.Add(("@" + ++i, DbType.UInt64, -1, value.ID));
            //                values += "@" + i + ", ";
            //            }

            //            query += columnNames + ") VALUES " + values + ");";
            //            SQLHelper.ExecuteNonQuery(query, parameters, path);
            //        }

            //        foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> savableGrouping in saveDict[SQLPropSaveType.MultipleChild].GroupBy(x => x.Item1.PropertyType))
            //            Save(savableGrouping.Select(x => x.Item1.GetValue(item) as ISavable ??
            //            throw new ArgumentException("Cannot declare an MULTI-CHILD relationship with an object that is not ISavable.")),
            //            item.ID, SQLPropSaveType.MultipleChild);

            //        foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> savableGrouping in saveDict[SQLPropSaveType.Link].GroupBy(x => x.Item1.PropertyType))
            //        {
            //            Save(savableGrouping.Select(x => x.Item1.GetValue(item) as ISavable ??
            //            throw new ArgumentException("Cannot declare an MULTI-CHILD relationship with an object that is not ISavable.")),
            //            item.ID, SQLPropSaveType.MultipleChild);
            //        }

            //        foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.Link])
            //        {
            //            var value = prop.GetValue(item) as ISavable;
            //            if (String.IsNullOrWhiteSpace(att.CustomColumnName))
            //                SaveLinkObject(String.Join("-", item.GetType().Name, prop.PropertyType.Name),
            //                    (item.GetType().Name, item.ID), (prop.PropertyType.Name, value.ID));
            //            else
            //                SaveLinkObject(String.Join("-", item.GetType().Name, prop.PropertyType.Name),
            //                    (item.GetType().Name, item.ID), (prop.PropertyType.Name, value.ID),
            //                    (att.CustomColumnName, att.CustomColumnValue.Invoke()));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        aex.Add((item, ex));
            //    }
            //}

            //if (aex.Any())
            //    throw new AggregateException(String.Join(Environment.NewLine, aex.Select(x => x.Item1.ID)), aex.Select(x => x.Item2));

            return loaded;
        }

        public void Save<T>(IEnumerable<T> items, long parentId = -1, SQLPropSaveType savingAs = SQLPropSaveType.Value)
            where T : ISavable
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

            var aex = new List<(ISavable, Exception)>();

            var saveDict = new Dictionary<SQLPropSaveType, List<(PropertyInfo, SQLPropAttribute)>>() {
                    { SQLPropSaveType.Link, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
                    { SQLPropSaveType.MultipleChild, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
                    { SQLPropSaveType.MultipleParent, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
                    { SQLPropSaveType.Value, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() }
                };

            foreach (PropertyInfo prop in properties)
            {
                var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
                saveDict[attribute.SaveRelationship].Add((prop, attribute));
            }

            foreach (T item in items)
            {
                //Save in the following order:
                //multiParents
                //Self with values and multiparentids inserted
                //multiChilds
                //link
                CheckInsertId(item);
                try
                {
                    foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.MultipleParent])
                        Save(GetUseableObjectsFromProp(item, prop), item.ID, SQLPropSaveType.MultipleParent);

                    {
                        string query = "INSERT INTO @TableName ";
                        string columnNames = "(";
                        string values = "(";
                        var parameters = new List<(string, DbType, int, object)>();
                        int i = 0;

                        foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.Value])
                        {
                            parameters.Add(("@" + ++i, DbType.String, -1, prop.Name));
                            columnNames += "@" + i + ", ";

                            object value = prop.GetValue(item);
                            parameters.Add(("@" + ++i, toDbType(att.SaveType), -1, value));
                            values += "@" + i + ", ";
                        }

                        foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.MultipleParent])
                        {
                            parameters.Add(("@" + ++i, DbType.String, -1, prop.Name));
                            columnNames += "@" + i + ", ";

                            var value = prop.GetValue(item) as ISavable;
                            parameters.Add(("@" + ++i, DbType.UInt64, -1, value.ID));
                            values += "@" + i + ", ";
                        }

                        query += columnNames.Remove(", ".Length) + ") VALUES "
                            + values.Remove(", ".Length) + ");";
                        SQLHelper.ExecuteNonQuery(query, parameters, path);
                    }

                    foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.MultipleChild]
                                                                  .Concat(saveDict[SQLPropSaveType.Link]))
                        Save(GetUseableObjectsFromProp(item, prop), item.ID, SQLPropSaveType.MultipleChild);

                    foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.Link])
                        foreach (ISavable value in GetUseableObjectsFromProp(item, prop))
                            if (String.IsNullOrWhiteSpace(att.CustomColumnName))
                                SaveLinkObject(String.Join("-", item.GetType().Name, prop.PropertyType.Name),
                                    (item.GetType().Name, item.ID), (prop.PropertyType.Name, value.ID));
                            else
                                SaveLinkObject(String.Join("-", item.GetType().Name, prop.PropertyType.Name),
                                    (item.GetType().Name, item.ID), (prop.PropertyType.Name, value.ID),
                                    (att.CustomColumnName, att.CustomColumnValue.Invoke()));
                }
                catch (Exception ex)
                {
                    aex.Add((item, ex));
                }
            }

            if (aex.Any())
                throw new AggregateException(String.Join(Environment.NewLine, aex.Select(x => x.Item1.ID)), aex.Select(x => x.Item2));
        }

        private static IEnumerable<ISavable> GetUseableObjectsFromProp<T>(T item, PropertyInfo prop) where T : ISavable
        {
            IEnumerable<ISavable> operable = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                ? (prop.GetValue(item) as IEnumerable).Cast<ISavable>()
                : prop.GetValue(item).Yield().Cast<ISavable>();
            // new[] { prop.GetValue(item) as ISavable };

            return operable ?? throw new Exception("Cannot declare a child relationship on an object that is not ISavable");
        }

        private static Type GetUseableTypeFromProp(PropertyInfo prop)
        {
            Type actual = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)
                ? prop.PropertyType.GenericTypeArguments.FirstOrDefault()
                : prop.PropertyType;

            return actual;
        }

        private DbType toDbType(SQLSaveType saveType)
        {
            switch (saveType)
            {
                case SQLSaveType.Blob:
                    return DbType.Object;
                case SQLSaveType.Real:
                    return DbType.Double;
                case SQLSaveType.Text:
                    return DbType.String;
                case SQLSaveType.Integer:
                    return DbType.Int32;
                default:
                    return DbType.String;
            }
        }

        private bool SaveLinkObject(string tableName, (string columnName, long Id) parent, (string columnName, long Id) child)
        {
            IdCheck(parent.Id, child.Id);
            string query = "INSERT INTO @TableName (@parentIdColumn, @childIdColumn) VALUES (@parentId, @childId);";
            var collection = new List<(string, DbType, int, object)>()
            {
                ("@TableName", DbType.String, -1, tableName),
                ("@parentIdColumn", DbType.String, -1, parent.columnName),
                ("@childIdColumn", DbType.String, -1, child.columnName),
                ("@parentId", DbType.UInt64, -1, parent.Id),
                ("@childId", DbType.UInt64, -1, child.Id)
            };
            return SQLHelper.ExecuteNonQuery(query, collection, path) > 1;
        }

        private bool SaveLinkObject(string tableName, (string columnName, long Id) parent, (string columnName, long Id) child,
            (string columnName, object value) customParam)
        {
            IdCheck(parent.Id, child.Id);
            string query = @"INSERT INTO @TableName (@parentIdColumn, @childIdColumn, @customParamColumn)
                                             VALUES (@parentId, @childId, @customParamValue);";
            var collection = new List<(string, DbType, int, object)>()
            {
                ("@TableName", DbType.String, -1, tableName),
                ("@parentIdColumn", DbType.String, -1, parent.columnName),
                ("@childIdColumn", DbType.String, -1, child.columnName),
                ("@parentId", DbType.UInt64, -1, parent.Id),
                ("@childId", DbType.UInt64, -1, child.Id),
                ("@customParamColumn", DbType.String, -1, customParam.columnName),
                ("@customParamValue", DbType.String, -1, customParam.value)
            };
            return SQLHelper.ExecuteNonQuery(query, collection, path) > 1;
        }

        //private IEnumerable<(string command, SQLiteParameterCollection parameters)> GetSaveCommand<T>(T item)
        //{
        //    IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
        //    foreach (PropertyInfo prop in properties)
        //    {
        //        var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
        //        object value = prop.GetValue(item);

        //        if (value == null && !attribute.Nullable)
        //            throw new NullReferenceException(item + ", " + prop.Name);
        //    }
        //}

        //private IEnumerable<(string command, SQLiteParameterCollection parameters)> GetLoadCommand<T>(T item)
        //{
        //    IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
        //    foreach (PropertyInfo prop in properties)
        //    {
        //        var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
        //    }
        //}

        private void CheckInsertId(params ISavable[] savables)
        {
            foreach (ISavable sav in savables)
                if (!IdCheck(sav.ID))
                    sav.ID = NextId();
        }

        private bool IdCheck(params long[] ids)
        {
            foreach (long id in ids)
                if (id < 0)
                    return false; //throw new InvalidOperationException("Attempted to write and Id out of the range of the allowed Ids");
            return true;
        }

        /// <summary>
        /// Determines if the databases point to the same database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>true if points to the same Database file (WARNING will return true no
        /// matter the state of the caches)</returns>
        public override bool Equals(object obj)
        {
            return obj is Database database && path == database.path;
        }

        public override int GetHashCode()
        {
            return -1757656154 + EqualityComparer<string>.Default.GetHashCode(path);
        }

        #endregion Methods
    }
}
