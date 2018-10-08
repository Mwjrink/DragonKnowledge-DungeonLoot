using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace DKDG.Utils
{
    public static class SQLHelper
    {
        private static readonly ReaderWriterLockSlim accessLock = new ReaderWriterLockSlim();

        private const bool DEBUG_SQL_STATEMENTS = true;

        private static readonly string commandWrapper =
            @"BEGIN TRY
                  BEGIN TRAN
                      {0}
                  COMMIT TRAN
              END TRY
              BEGIN CATCH
                  IF(@@TRANCOUNT > 0)
                      ROLLBACK TRAN;
                  THROW;
              END CATCH";

        #region Methods

        private static T CommandPrep<T>(string query, IEnumerable<(string, DbType, int, object)> sqlParameterCollection,
            Func<SQLiteCommand, T> action, string path = null)
        {
            try
            {
                if (!DEBUG_SQL_STATEMENTS)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    using (var conn = new SQLiteConnection(path ?? INIValuesStatic.DB_PATH))
                    {
                        conn.Open();

                        var command = new SQLiteCommand(String.Format(commandWrapper, query), conn);


                        foreach ((string parameterName, DbType parameterType, int parameterSize, object value) in sqlParameterCollection)
                            if (parameterSize < 1)
                                command.Parameters.Add(new SQLiteParameter(parameterName, parameterType) { Value = value });
                            else
                                command.Parameters.Add(new SQLiteParameter(parameterName, parameterType, parameterSize) { Value = value });

                        return action.Invoke(command);
                    }
#pragma warning restore CS0162 // Unreachable code detected
                }
                else
                {
                    using (var fl = new FileStream(path ?? INIValuesStatic.DB_PATH, FileMode.OpenOrCreate))
                    using (var tw = new StreamWriter(fl))
                    {
                        string New = query;
                        foreach ((string, DbType, int, object) v in sqlParameterCollection)
                            New.Replace(v.Item1, v.Item4?.ToString());
                        tw.WriteLine(DateTime.Now);
                        tw.WriteLine();
                        tw.WriteLine(New);
                        tw.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        private static T CommandPrep<T>(string query, SQLiteParameterCollection sqlParameterCollection,
            Func<SQLiteCommand, T> action, string path = null)
        {
            try
            {
                if (!DEBUG_SQL_STATEMENTS)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    using (var conn = new SQLiteConnection(path ?? INIValuesStatic.DB_PATH))
                    {
                        conn.Open();

                        var command = new SQLiteCommand(String.Format(commandWrapper, query), conn);
                        command.Parameters.Add(sqlParameterCollection);

                        return action.Invoke(command);
                    }
#pragma warning restore CS0162 // Unreachable code detected
                }
                else
                {
                    using (var fl = new FileStream(path ?? INIValuesStatic.DB_PATH, FileMode.OpenOrCreate))
                    using (var tw = new StreamWriter(fl))
                    {
                        string New = query;
                        foreach ((string, DbType, int, object) v in sqlParameterCollection)
                            New.Replace(v.Item1, (string)v.Item4);
                        tw.WriteLine(DateTime.Now);
                        tw.WriteLine();
                        tw.WriteLine(New);
                        tw.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                        return default(T);
                    }
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static int ExecuteNonQuery(string query, IEnumerable<(string, DbType, int, object)> sqlParameterCollection,
            string path = null)
        {
            return CommandPrep(query, sqlParameterCollection, command =>
            {
                using (accessLock.Write())
                    return command.ExecuteNonQuery();
            }, path);
        }

        //public static int ExecuteNonQuery(string query, SQLiteParameterCollection sqlParameterCollection, string path = null)
        //{
        //    return CommandPrep(query, sqlParameterCollection, command =>
        //    {
        //        using (accessLock.Write())
        //            return command.ExecuteNonQuery();
        //    }, path);
        //}

        public static DataTable ExecuteQuery(string query, IEnumerable<(string, DbType, int, object)> sqlParameterCollection,
            string path = null)
        {
            return CommandPrep(query, sqlParameterCollection, command =>
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    var result = new DataTable();

                    using (accessLock.Read())
                        result.Load(reader);

                    return result;
                }
            }, path);
        }

        //public static DataTable ExecuteQuery(string query, SQLiteParameterCollection sqlParameterCollection, string path = null)
        //{
        //    return CommandPrep(query, sqlParameterCollection, command =>
        //    {
        //        using (SQLiteDataReader reader = command.ExecuteReader())
        //        {
        //            var result = new DataTable();

        //            using (accessLock.Read())
        //                result.Load(reader);

        //            return result;
        //        }
        //    }, path);
        //}

        #endregion Methods
    }
}

/*
public static string stringer =
@"DECLARE @first_table AS TABLE(col1 int IDENTITY, col_x varchar(20), col_y varchar(20))
DECLARE @second_table AS TABLE(col2 int IDENTITY, col_z varchar(20), col_a varchar(20))
DECLARE @third_table AS TABLE(col3 int IDENTITY, col_b varchar(20), col_c varchar(20))
DECLARE @my_table AS TABLE(col1 int, col2 int, col3 int, col_v varchar(20))
DECLARE @col1 int
DECLARE @col2 int
DECLARE @col3 int

INSERT INTO dbo.Account(AccountId, Name , Balance) VALUES(1, 'Account1',  10000)
UPDATE dbo.Account SET Balance = Balance + CAST('TEN THOUSAND' AS MONEY) WHERE AccountId = 1
INSERT INTO dbo.Account(AccountId, Name , Balance) VALUES(2, 'Account2',  20000)";
*/
