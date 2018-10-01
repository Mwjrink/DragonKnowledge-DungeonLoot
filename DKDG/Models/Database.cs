using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            T loaded = Activator.CreateInstance<T>();

            typeof(T).GetProperties();

            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
        }

        private bool Save<T>(IEnumerable<T> items, long parentId = -1, SQLPropSaveType savingAs = SQLPropSaveType.Value) where T : ISaveable
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
            foreach (T item in items)
            {
                string command = "";
                var parameters = new List<SQLiteParameter>();
                foreach (PropertyInfo prop in properties)
                {
                    var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
                    object value = prop.GetValue(item);

                    if (value == null && !attribute.Nullable)
                        throw new NullReferenceException(item + ", " + prop.Name);

                    switch (attribute.SaveRelationship)
                    {
                        case SQLPropSaveType.Link:

                            break;

                        case SQLPropSaveType.MultipleChild: //TODO Max, uh
                            typeof(Database).GetMethod("Save").MakeGenericMethod(prop.PropertyType).Invoke(this, new object[] { new[] { value }, item.ID, SQLPropSaveType.MultipleChild });
                            Save<prop.PropertyType>(value.Yield(), item.ID, SQLPropSaveType.MultipleChild);
                            break;

                        case SQLPropSaveType.MultipleParent:

                            break;

                        case SQLPropSaveType.Value:

                            break;
                    }
                }

                SQLHelper.ExecuteNonQuery(command, parameters, path);
            }
        }

        private IEnumerable<(string command, SQLiteParameterCollection parameters)> GetSaveCommand<T>(T item)
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
            foreach (PropertyInfo prop in properties)
            {
                var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
                object value = prop.GetValue(item);

                if (value == null && !attribute.Nullable)
                    throw new NullReferenceException(item + ", " + prop.Name);
            }
        }

        private IEnumerable<(string command, SQLiteParameterCollection parameters)> GetLoadCommand<T>(T item)
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
            foreach (PropertyInfo prop in properties)
            {
                var attribute = (SQLPropAttribute)prop.GetCustomAttributes(typeof(SQLPropAttribute), false).FirstOrDefault();
            }
        }

        //using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=D:\test.db;"))
        //{
        //    conn.Open();

        // SQLiteCommand command = new SQLiteCommand("Select * from yourTable", conn);
        // SQLiteDataReader reader = command.ExecuteReader();

        // while (reader.Read()) Console.WriteLine(reader["YourColumn"]);

        //    reader.Close();
        //}

        #endregion Methods
    }
}
