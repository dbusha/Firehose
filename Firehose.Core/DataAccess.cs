using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        public async Task CreateTables()
        {
            var query = File.ReadAllText("build_db_script.sql");
            await ExecuteNonQueryAsync_(query);
        }

        
        public async Task<int?> AddFeedAsync(string name, string address)
        {
            var id = GetNextFeedId_();
            if (id == null)
                return null;
            var insertQuery = $"insert into feeds select {id}, '{name}', '{address}'";
            int result = await ExecuteNonQueryAsync_(insertQuery);
            return result == 1 ? id : null;
        }


        public async Task<bool> RenameFeedAsync(int id, string newName)
        {
            var renameQuery = $"update feeds set name = '{newName}' where id = {id}";
            return await ExecuteNonQueryAsync_(renameQuery) == 1;
        }


        public async Task<bool> DeleteFeedAsync(int id)
        {
            var deleteQuery = $"delete from feeds where id={id}";
            return await ExecuteNonQueryAsync_(deleteQuery) == 1;
        }


        private int? GetNextFeedId_()
        {
            var selectQuery = "select id from feeds where id=max(id);";
            var reader = ExecuteQueryAsync_(selectQuery).Result;
            if (reader == null || !reader.HasRows)
                return null;
            return reader.GetInt32(0);
        }
        
        
        private async Task<int> ExecuteNonQueryAsync_(string query)
        {
            var cxn = new SqliteConnection($"Data Source={databaseName}");

            try {
                cxn.Open();
                var cmd = cxn.CreateCommand();
                cmd.CommandText = query;
                return await cmd.ExecuteNonQueryAsync();
            } finally {
                if (cxn != null && cxn.State != System.Data.ConnectionState.Broken)
                    cxn.Close();
            }
         }

        
        private async Task<SqliteDataReader> ExecuteQueryAsync_(string query)
        {
            try {
                var cxn = new SqliteConnection(connectionString_);
                cxn.Open();
                var cmd = cxn.CreateCommand();
                cmd.CommandText = query;
                return await cmd.ExecuteReaderAsync();
            } catch (Exception err) {
                logger_.Error(err.InnerException?.Message ?? err.Message);
                return null;
            } 
        }

        
        public async Task UpdateFeedAsync(FeedItem feedItem)
        {
            throw new NotImplementedException();
        }
    }
}