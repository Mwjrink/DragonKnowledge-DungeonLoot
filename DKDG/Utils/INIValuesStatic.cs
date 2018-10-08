using System;
using System.IO;
using System.Reflection;

namespace DKDG.Utils
{
    public static class INIValuesStatic
    {
        #region Properties

        public static string DB_PATH { get; private set; } = "Main.db";

        #endregion Properties

        #region Constructors

        static INIValuesStatic()
        {
            string PATH = "settings.ini";
            bool create = !File.Exists(PATH);

            using (var file = new FileStream(PATH, FileMode.OpenOrCreate))
            {
                if (!create)
                {
                    using (var reader = new StreamReader(file))
                    {
                        string line;
                        while (!String.IsNullOrEmpty(line = reader.ReadLine()))
                        {
                            try
                            {
                                string[] parsed = line.Split('=');
                                typeof(INIValuesStatic).GetProperty(parsed[0].Trim(), BindingFlags.Static).SetValue(null, parsed[1].Trim());
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        reader.Close();
                    }
                }
                else
                {
                    using (var writer = new StreamWriter(file))
                    {
                        foreach (PropertyInfo prop in typeof(INIValuesStatic).GetProperties(BindingFlags.Static))
                            writer.WriteLine(prop.Name + " = " + prop.GetValue(null));

                        writer.Close();
                    }
                }

                file.Close();
            }
        }

        #endregion Constructors
    }
}
