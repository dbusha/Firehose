using System;
using System.IO;
using Microsoft.Data.Sqlite;
using NLog;

namespace Firehose.Core
{
    public class DataAccess
    {
        private string databaseName = "test.db";
        private string connectionString_;
        private Logger logger_ = LogManager.GetCurrentClassLogger();
        
        public DataAccess()
        {
            connectionString_ = $"DataSource = {databaseName}";
        }


        public void CreateDatabase()
        {
            if (!System.IO.File.Exists(databaseName))
            {
                using (var connection = new SqliteConnection(connectionString_))
                    connection.Open();
            }
        }

        public void CreateTables()
        {
            var query = File.ReadAllText("build_db_script.sql");
            ExecuteNonQuery_(query);
        }

        
        public int? AddFeed(string name, string address)
        {
            var id = GetNextFeedId_();
            if (id == null)
                return null;
            var insertQuery = $"insert into feeds select {id}, '{name}', '{address}'";
            int result = ExecuteNonQuery_(insertQuery);
            return result == 1 ? id : null;
        }


        public bool RenameFeed(int id, string newName)
        {
            var renameQuery = $"update feeds set name = '{newName}' where id = {id}";
            return ExecuteNonQuery_(renameQuery) == 1;
        }


        public bool DeleteFeed(int id)
        {
            var deleteQuery = $"delete from feeds where id={id}";
            return ExecuteNonQuery_(deleteQuery) == 1;
        }


        private int? GetNextFeedId_()
        {
            var selectQuery = "select id from feeds where id=max(id);";
            var reader = ExecuteQuery_(selectQuery);
            if (reader == null || !reader.HasRows)
                return null;
            return reader.GetInt32(0);
        }
        
        
        private int ExecuteNonQuery_(string query)
        {
            var cxn = new SqliteConnection($"Data Source={databaseName}");

            try {
                cxn.Open();
                var cmd = cxn.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            } finally {
                if (cxn != null && cxn.State != System.Data.ConnectionState.Broken)
                    cxn.Close();
            }
         }

        
        private SqliteDataReader ExecuteQuery_(string query)
        {
            try {
                var cxn = new SqliteConnection(connectionString_);
                cxn.Open();
                var cmd = cxn.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteReader();
            } catch (Exception err) {
                logger_.Error(err.InnerException?.Message ?? err.Message);
                return null;
            } 
        }
        
    }
}