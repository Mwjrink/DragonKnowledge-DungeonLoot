using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace DKDG.Utils.SavingLoading.SQL
{
    public static class SQLHelper
    {
        #region Methods

        private static T CommandSurround<T>(string query,
                    IEnumerable<(string parameterName, DbType parameterType, int parameterSize, object value)> sqlParameterCollection,
                    Func<SQLiteCommand, T> action)
        {
            try
            {
                using (var conn = new SQLiteConnection(INIValuesStatic.DB_PATH))
                {
                    conn.Open();

                    var command = new SQLiteCommand(query, conn);

                    foreach ((string parameterName, DbType parameterType, int parameterSize, object value) in sqlParameterCollection)
                        command.Parameters.Add(new SQLiteParameter(parameterName, parameterType, parameterSize) { Value = value });

                    return action.Invoke(command);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Execute the command and return the number of rows inserted/updated affected by it.
        /// </summary>
        /// <param name="query">The command to execute.</param>
        /// <param name="sqlParameterCollection">The parameters of the command.</param>
        /// <returns>The number of rows inserted/updated affected by it.</returns>
        public static int ExecuteNonQuery(string query, IEnumerable<(string parameterName, DbType parameterType, int parameterSize, object value)> sqlParameterCollection)
        {
            return CommandSurround(query, sqlParameterCollection, command => command.ExecuteNonQuery());
        }

        public static DataTable ExecuteQuery(string query,
            IEnumerable<(string parameterName, DbType parameterType, int parameterSize, object value)> sqlParameterCollection)
        {
            return CommandSurround(query, sqlParameterCollection, command =>
                        {
                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                var result = new DataTable();

                                result.Load(reader);

                                reader.Close();

                                return result;
                            }
                        });
        }

        #endregion Methods
    }
}
