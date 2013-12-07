using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using AzureLogSpelunker.Models;

namespace AzureLogSpelunker
{
    public class SqlCache
    {
        protected const string TableName = "LogCache";
        protected readonly SQLiteConnection Connection;
        protected readonly ISettings Settings;

        public SqlCache(ISettings settings)
        {
            Connection = InitializeDatabase();
            Settings = settings;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void PopulateCache(IEnumerable<LogEntity> resultSet)
        {
            var properties = Utility.GetProperties<LogEntity>();

            var columnSql = "";
            var valueSql = "";
            foreach (var info in properties)
            {
                columnSql += columnSql == "" ? " (" : ", ";
                valueSql += valueSql == "" ? " (" : ", ";
                columnSql += info.Name;
                valueSql += "@" + info.Name;
            }
            var sql = "INSERT INTO " + TableName + columnSql + ") VALUES" + valueSql + ")";
            var preparedQuery = new SQLiteCommand(sql, Connection);
            foreach (var info in properties)
            {
                preparedQuery.Parameters.Add("@" + info.Name, SqliteDbType(info.PropertyType));
            }

            using (var transaction = Connection.BeginTransaction())
            {
                ClearCache();

                foreach (var result in resultSet)
                {
                    foreach (var info in properties)
                    {
                        preparedQuery.Parameters["@" + info.Name].Value = info.GetValue(result);
                    }
                    preparedQuery.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public long CacheCount()
        {
            const string sql = "SELECT COUNT(*) FROM " + TableName;
            using (var query = new SQLiteCommand(sql, Connection))
            {
                return (long)query.ExecuteScalar();
            }
        }

        //This method is vulnerable to SQL injection attacks, but that's kinda the point of this application.
        public string ApplyFilters(DataTable dataTable)
        {
            var sql = ComputeSql();
            var da = new SQLiteDataAdapter(sql, Connection);
            da.Fill(dataTable);
            dataTable.TableName = "LogEntity";
            return sql;
        }

        public string ComputeSql()
        {
            return "SELECT RowId,* FROM " + TableName + WhereClause();
        }

        private void ClearCache()
        {
            const string deleteSql = "DELETE FROM " + TableName;
            var deleteQuery = new SQLiteCommand(deleteSql, Connection);
            deleteQuery.ExecuteNonQuery();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static SQLiteConnection InitializeDatabase()
        {
            const string connectionString = "Data Source=':memory:';";
            var connection = new SQLiteConnection(connectionString).OpenAndReturn();

            var properties = Utility.GetProperties<LogEntity>();

            var sql = "CREATE TABLE " + TableName;
            bool first = true;
            foreach (var info in properties)
            {
                sql += first ? " (" : ", ";
                sql += info.Name;
                sql += " ";
                sql += SqliteTextType(info.PropertyType);
                first = false;
            }
            sql += ", PRIMARY KEY (PartitionKey, RowKey)";
            sql += ")";
            new SQLiteCommand(sql, connection).ExecuteNonQuery();

            return connection;
        }

        private static string SqliteTextType(Type t)
        {
            var dbType = SqliteDbType(t);
            if (dbType == DbType.String) return "TEXT";
            if (dbType == DbType.Int32) return "INT";
            if (dbType == DbType.Int64) return "BIGINT";
            throw new ApplicationException("Unexpected type: " + t);
        }

        private static DbType SqliteDbType(Type t)
        {
            if (t == typeof(string)) return DbType.String;
            if (t == typeof(int)) return DbType.Int32;
            if (t == typeof(Int16)) return DbType.Int32;
            if (t == typeof(Int32)) return DbType.Int32;
            if (t == typeof(Int64)) return DbType.Int64;
            if (t == typeof(DateTimeOffset)) return DbType.String;  //The best I can do
            throw new ApplicationException("Unexpected type: " + t);
        }

        private string WhereClause()
        {
            var whereClause = String.Empty;
            foreach (var filter in Settings.GetFilters())
            {
                if (filter.Active)
                {
                    if (String.IsNullOrEmpty(whereClause))
                        whereClause = " WHERE ";
                    else
                        whereClause += " AND ";
                    whereClause += filter.FilterText;
                }
            }
            return whereClause;
        }

    }
}
