using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DKDG.Utils;

namespace DKDG.Models
{
    public static class StaticDatabaseCache
    {
        #region Fields

        private static readonly Dictionary<long, Armor> loadedArmor = new Dictionary<long, Armor>();
        private static readonly Dictionary<long, Background> loadedBackground = new Dictionary<long, Background>();
        private static readonly Dictionary<long, Character> loadedCharacter = new Dictionary<long, Character>();
        private static readonly Dictionary<long, Class> loadedClass = new Dictionary<long, Class>();
        private static readonly Dictionary<long, Language> loadedLanguage = new Dictionary<long, Language>();
        private static readonly Dictionary<long, Monster> loadedMonster = new Dictionary<long, Monster>();
        private static readonly Dictionary<long, NPC> loadedNPC = new Dictionary<long, NPC>();
        private static readonly Dictionary<long, OtherEquipment> loadedOtherEquipment = new Dictionary<long, OtherEquipment>();
        private static readonly Dictionary<long, Race> loadedRace = new Dictionary<long, Race>();
        private static readonly Dictionary<long, Skill> loadedSkill = new Dictionary<long, Skill>();
        private static readonly Dictionary<long, Spell> loadedSpells = new Dictionary<long, Spell>();
        private static readonly Dictionary<long, Tool> loadedTool = new Dictionary<long, Tool>();
        private static readonly Dictionary<long, Weapon> loadedWeapon = new Dictionary<long, Weapon>();

        public static object SQLConnection { get; private set; }

        #endregion Fields

        #region Methods

        private static void GenericFillDict<T>()
        {
            //get dict to load into
        }

        private static ISaveable Load<T>(long ID) where T : new()
        {
            //get properties in class and load
            Activator.CreateInstance<T>();

            typeof(T).GetProperties();

            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
            PropertyInfo prop = properties.First();
            var attributes = (SQLPropAttribute[])prop.GetCustomAttributes(typeof(SQLPropAttribute), false);
        }

        private static bool Save<T>(IEnumerable<T> item)
        {
            List < (string query, ) >;
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.IsDefined(typeof(SQLPropAttribute), false));
            PropertyInfo prop = properties.First();
            var attributes = (SQLPropAttribute[])prop.GetCustomAttributes(typeof(SQLPropAttribute), false);

            SQLConnection
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
