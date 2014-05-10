using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using AzureLogSpelunker.Models;

namespace AzureLogSpelunker
{
    public interface ISqlCache
    {
        long PopulateCache(IEnumerable<LogEntity> resultSet, ISettings settings);
        string ApplyFilters(DataTable dataTable, ISettings settings);
        string ComputeSql(ISettings settings);
        void Close();
    }

    public class SqlCache : ISqlCache
    {
        protected const string TableName = "LogCache";
        protected const string DiskCacheFile = "Cache.db";
        protected SQLiteConnection Connection = null;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public long PopulateCache(IEnumerable<LogEntity> resultSet, ISettings settings)
        {
            Close();
            Connection = GetConnection(settings);
            InitializeDatabase(Connection);

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

            return CacheCount(Connection);
        }

        //This method is vulnerable to SQL injection attacks, but that's kinda the point of this application.
        public string ApplyFilters(DataTable dataTable, ISettings settings)
        {
            var sql = ComputeSql(settings);
            if (Connection != null)
            {
                var da = new SQLiteDataAdapter(sql, Connection);
                da.Fill(dataTable);
            }
            dataTable.TableName = "LogEntity";
            return sql;
        }

        public string ComputeSql(ISettings settings)
        {
            return "SELECT RowId,* FROM " + TableName + WhereClause(settings);
        }

        public void Close()
        {
            if (Connection == null || Connection.State == ConnectionState.Closed) return;
            ClearCache(Connection);
            Connection.Close();
        }

        private static SQLiteConnection GetConnection(ISettings settings)
        {
            string connectionString;
            switch (settings.GetFetchTo())
            {
                case "disk":
                    var myCacheFilename = Path.Combine(settings.MyApplicationDataPath, DiskCacheFile);
                    connectionString = "Data Source='" + myCacheFilename + "';";
                    break;
                default:
                    connectionString = "Data Source=':memory:';";
                    break;
            }
            return new SQLiteConnection(connectionString).OpenAndReturn();
        }

        private static void ClearCache(SQLiteConnection connection)
        {
            const string deleteSql = "DROP TABLE IF EXISTS " + TableName;
            var deleteQuery = new SQLiteCommand(deleteSql, connection);
            deleteQuery.ExecuteNonQuery();

            const string vacuumSql = "VACUUM";
            var vacuumQuery = new SQLiteCommand(vacuumSql, connection);
            vacuumQuery.ExecuteNonQuery();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static void InitializeDatabase(SQLiteConnection connection)
        {
            ClearCache(connection);

            var properties = Utility.GetProperties<LogEntity>();

            var sql = "CREATE TABLE " + TableName;
            var first = true;
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
        }

        private static long CacheCount(SQLiteConnection connection)
        {
            const string sql = "SELECT COUNT(*) FROM " + TableName;
            using (var query = new SQLiteCommand(sql, connection))
            {
                return (long)query.ExecuteScalar();
            }
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

        private static string WhereClause(ISettings settings)
        {
            var whereClause = String.Empty;
            foreach (var filter in settings.GetFilters())
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
