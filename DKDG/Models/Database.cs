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

        private void GenericFillDict<T>()
        {
            //get dict to load into
        }

        private ISaveable Load<T>(long ID) where T : new()
        {
            //get properties in class and load
            var loaded = Activator.CreateInstance<T>();

            typeof(T).GetProperties();

            var properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
        }

        private bool Save<T>(IEnumerable<T> items, long parentId = -1, SQLPropSaveType savingAs = SQLPropSaveType.Value)
            where T : ISaveable
        {
            var properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));

            var saveDict = new Dictionary<SQLPropSaveType, List<(PropertyInfo, SQLPropAttribute)>>() {
                    { SQLPropSaveType.Link, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
                    { SQLPropSaveType.MultipleChild, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
                    { SQLPropSaveType.MultipleParent, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() },
                    { SQLPropSaveType.Value, new List<(PropertyInfo prop, SQLPropAttribute attribute)>() }
                };

            foreach (var prop in properties)
            {
                var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
                saveDict[attribute.SaveRelationship].Add((prop, attribute));
            }

            foreach (var item in items)
            {
                //Save in the following order:
                //multiChild
                //Self with values and multiparentids inserted
                //link

                foreach (var saveableGrouping in saveDict[SQLPropSaveType.MultipleChild].GroupBy(x => x.Item1.PropertyType))
                    Save(saveableGrouping.Select(x => x.Item1.GetValue(item) as ISaveable ??
                    throw new ArgumentException("Cannot declare an ISCHILD relationship with an object that is not ISaveable.")),
                    item.ID);

                {
                    string query = "INSERT INTO @TableName (@parentIdColumn, @childIdColumn) VALUES (@parentId, @childId);";
                    string columnNames = "(";
                    string values = "(";
                    var parameters = new List<(string, DbType, int, object)>();

                    foreach ((var prop, var att) in saveDict[SQLPropSaveType.Value])
                    {
                        object value = prop.GetValue(item);
                        parameters.Add(("@" + prop.Name, toDbType(value), -1, value));
                    }

                    foreach ((var prop, var att) in saveDict[SQLPropSaveType.MultipleParent])
                    {
                        ///BLEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEHHHH
                        object value = prop.GetValue(item);
                        parameters.Add(("@" + prop.Name, toDbType(value), -1, value));
                    }

                    query += columnNames + ')' + values + ");";
                    SQLHelper.ExecuteNonQuery(query, collection, path);
                }

                SQLHelper.ExecuteNonQuery(command, parameters, path);
            }
        }

        private DbType toDbType(object value)
        {
            if (value is string)
                return DbType.String;
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

        private void IdCheck(params long[] ids)
        {
            foreach (long id in ids.Where(id => id < 0))
                throw new InvalidOperationException("Attempted to write and Id out of the range of the allowed Ids");
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
