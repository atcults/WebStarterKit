using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Common.Extensions;

namespace IntegrationTests.DataAccess
{
    public class SqlExtension
    {
        private readonly string _connectionString;

        public SqlExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CheckDatabaseExist()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT 1+1";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public int GetDatabaseVersion()
        {
            if (!CheckDatabaseExist()) return 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "Select Value from Configuration where ConfigName = 'SchemaVersion'";
                    return (int) command.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        ///     ResetDabse()
        ///     It is use for delete rows data from the all table.
        ///     Ignore "usd_AppliedDatabaseScript" table
        /// </summary>
        public void ResetDatabase()
        {
            var tableList = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT name  FROM sysobjects WHERE  xtype= 'U' ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableList.Add(reader["name"].ToString());
                    }
                }
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                if (!tableList.IsNotEmpty()) return;
                foreach (var table in tableList)
                {
                    if (string.Equals(table, "usd_AppliedDatabaseScript"))
                        continue;
                    var deleteDataCommand = connection.CreateCommand();
                    deleteDataCommand.CommandType = CommandType.Text;
                    deleteDataCommand.CommandText = string.Format("delete from {0}", table);
                    deleteDataCommand.ExecuteNonQuery();
                }
            }
        }

        //NOTE: {SR} Try to run all queries at once to get perfomance
        public void ExecuteSql(string sql)
        {
            if (string.IsNullOrEmpty(sql)) return;

            Console.WriteLine(sql);
            Console.WriteLine("GO");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteCore(string storeProcName, Action<SqlCommand> addCommandParams,
            Action<SqlCommand> commandResultAction)
        {
            Console.WriteLine("Executing: " + storeProcName);

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = storeProcName;

                    if (addCommandParams != null)
                        addCommandParams(command);

                    if (commandResultAction == null)
                        throw new InvalidOperationException("ExecuteCore called with invalid commandResult action.");

                    commandResultAction(command);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error while executing store proc: " + storeProcName, ex);
            }
        }
    }
}