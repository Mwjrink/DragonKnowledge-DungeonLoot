using System;
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
        private readonly Dictionary<long, Armor> loadedArmor = new Dictionary<long, Armor>();
        private readonly Dictionary<long, Background> loadedBackground = new Dictionary<long, Background>();
        private readonly Dictionary<long, Character> loadedCharacter = new Dictionary<long, Character>();
        private readonly Dictionary<long, Class> loadedClass = new Dictionary<long, Class>();
        private readonly Dictionary<long, Language> loadedLanguage = new Dictionary<long, Language>();
        private readonly Dictionary<long, Monster> loadedMonster = new Dictionary<long, Monster>();
        private readonly Dictionary<long, NPC> loadedNPC = new Dictionary<long, NPC>();
        private readonly Dictionary<long, OtherEquipment> loadedOtherEquipment = new Dictionary<long, OtherEquipment>();
        private readonly Dictionary<long, Race> loadedRace = new Dictionary<long, Race>();
        private readonly Dictionary<long, Skill> loadedSkill = new Dictionary<long, Skill>();
        private readonly Dictionary<long, Spell> loadedSpells = new Dictionary<long, Spell>();
        private readonly Dictionary<long, Tool> loadedTool = new Dictionary<long, Tool>();
        private readonly Dictionary<long, Weapon> loadedWeapon = new Dictionary<long, Weapon>();
        public static readonly long DEFAULT_ID = -1;

        #endregion Fields

        #region Constructors

        public Database(string path)
        {
            this.path = path;
        }

        #endregion Constructors

        #region Methods

        public int nextId()
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

        private void GenerateTables(IEnumerable<Type> savables, HashSet<string> generated = null)
        {
            if (generated == null)
                generated = new HashSet<string>();

            var mps = new List<PropertyInfo>();
            var mcs = new List<PropertyInfo>();
            var lks = new List<PropertyInfo>();
            var vls = new List<PropertyInfo>();

            foreach (Type savable in savables)
                if (generated.Add(savable.Name))
                {
                    IEnumerable<PropertyInfo> properties = savable.GetType().GetProperties()
                                                            .Where(p => p.IsDefined(typeof(SQLPropAttribute), false));


                    int i = 0;
                    string query = "";
                    var parameters = new List<(string, DbType, int, object)>();



                    query += "CREATE TABLE @" + i + "(";
                    parameters.Add(("@" + i, DbType.String, -1, savable.GetType().Name));

                    //TODO Max, make sure this gives "ID" not "ISaveable.ID"
                    query += "@" + ++i;
                    parameters.Add(("@" + i, DbType.String, -1, nameof(ISaveable.ID)));


                    //Generate in the following order
                    //multiParent
                    //this & value
                    //multichild
                    foreach (PropertyInfo prop in properties)
                    {
                        if (prop.Name == "ID")
                            continue;

                        var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
                        query += "@" + ++i;

                        switch (attribute.SaveRelationship)
                        {
                            case SQLPropSaveType.MultipleParent:
                                mps.Add(prop);
                                parameters.Add(("@" + i, DbType.String, -1, prop.Name));
                                break;
                            case SQLPropSaveType.Link:
                                lks.Add(prop);
                                break;
                            case SQLPropSaveType.MultipleChild:
                                mcs.Add(prop);
                                break;
                            case SQLPropSaveType.Value:
                                vls.Add(prop);
                                parameters.Add(("@" + i, DbType.String, -1, prop.Name));
                                break;
                        }
                    }

                    query += ");";

                    GenerateTables(mps.Select(x => x.PropertyType), generated);

                    SQLHelper.ExecuteNonQuery(query, parameters, path);

                    GenerateTables(lks.Select(x => x.PropertyType), generated);

                    //gen the link tables

                    GenerateTables(mcs.Select(x => x.PropertyType), generated);

                    mps.Clear();
                    mcs.Clear();
                    lks.Clear();
                }
        }

        private ISaveable Load<T>(long ID)
            where T : ISaveable, new()
        {
            ////get properties in class and load
            T loaded = Activator.CreateInstance<T>();

            //IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

            //var aex = new List<(ISaveable, Exception)>();

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
            //        foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> saveableGrouping in saveDict[SQLPropSaveType.MultipleParent].GroupBy(x => x.Item1.PropertyType))
            //            Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
            //            throw new ArgumentException("Cannot declare a MULTI-PARENT relationship with an object that is not ISaveable.")),
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

            //                var value = prop.GetValue(item) as ISaveable;
            //                parameters.Add(("@" + ++i, DbType.UInt64, -1, value.ID));
            //                values += "@" + i + ", ";
            //            }

            //            query += columnNames + ") VALUES " + values + ");";
            //            SQLHelper.ExecuteNonQuery(query, parameters, path);
            //        }

            //        foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> saveableGrouping in saveDict[SQLPropSaveType.MultipleChild].GroupBy(x => x.Item1.PropertyType))
            //            Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
            //            throw new ArgumentException("Cannot declare an MULTI-CHILD relationship with an object that is not ISaveable.")),
            //            item.ID, SQLPropSaveType.MultipleChild);

            //        foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> saveableGrouping in saveDict[SQLPropSaveType.Link].GroupBy(x => x.Item1.PropertyType))
            //        {
            //            Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
            //            throw new ArgumentException("Cannot declare an MULTI-CHILD relationship with an object that is not ISaveable.")),
            //            item.ID, SQLPropSaveType.MultipleChild);
            //        }

            //        foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.Link])
            //        {
            //            var value = prop.GetValue(item) as ISaveable;
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

        private void Save<T>(IEnumerable<T> items, long parentId = -1, SQLPropSaveType savingAs = SQLPropSaveType.Value)
            where T : ISaveable
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

            var aex = new List<(ISaveable, Exception)>();

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
                CheckInsert(item);
                try
                {
                    foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> saveableGrouping in saveDict[SQLPropSaveType.MultipleParent].GroupBy(x => x.Item1.PropertyType))
                        Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
                        throw new ArgumentException("Cannot declare a MULTI-PARENT relationship with an object that is not ISaveable.")),
                        item.ID, SQLPropSaveType.MultipleParent);

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

                            var value = prop.GetValue(item) as ISaveable;
                            parameters.Add(("@" + ++i, DbType.UInt64, -1, value.ID));
                            values += "@" + i + ", ";
                        }

                        query += columnNames.Remove(", ".Length) + ") VALUES "
                            + values.Remove(", ".Length) + ");";
                        SQLHelper.ExecuteNonQuery(query, parameters, path);
                    }

                    foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> saveableGrouping in saveDict[SQLPropSaveType.MultipleChild].GroupBy(x => x.Item1.PropertyType))
                        Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
                        throw new ArgumentException("Cannot declare an MULTI-CHILD relationship with an object that is not ISaveable.")),
                        item.ID, SQLPropSaveType.MultipleChild);

                    foreach (IGrouping<Type, (PropertyInfo, SQLPropAttribute)> saveableGrouping in saveDict[SQLPropSaveType.Link].GroupBy(x => x.Item1.PropertyType))
                    {
                        Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
                        throw new ArgumentException("Cannot declare an MULTI-CHILD relationship with an object that is not ISaveable.")),
                        item.ID, SQLPropSaveType.MultipleChild);
                    }

                    foreach ((PropertyInfo prop, SQLPropAttribute att) in saveDict[SQLPropSaveType.Link])
                    {
                        var value = prop.GetValue(item) as ISaveable;
                        if (String.IsNullOrWhiteSpace(att.CustomColumnName))
                            SaveLinkObject(String.Join("-", item.GetType().Name, prop.PropertyType.Name),
                                (item.GetType().Name, item.ID), (prop.PropertyType.Name, value.ID));
                        else
                            SaveLinkObject(String.Join("-", item.GetType().Name, prop.PropertyType.Name),
                                (item.GetType().Name, item.ID), (prop.PropertyType.Name, value.ID),
                                (att.CustomColumnName, att.CustomColumnValue.Invoke()));
                    }
                }
                catch (Exception ex)
                {
                    aex.Add((item, ex));
                }
            }

            if (aex.Any())
                throw new AggregateException(String.Join(Environment.NewLine, aex.Select(x => x.Item1.ID)), aex.Select(x => x.Item2));
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

        private void CheckInsert(params ISaveable[] savables)
        {
            foreach (ISaveable sav in savables)
                if (!IdCheck(sav.ID))
                    sav.ID = nextId();
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
