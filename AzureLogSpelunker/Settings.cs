using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using AzureLogSpelunker.Models;

namespace AzureLogSpelunker
{

    public interface ISettings
    {
        IList<string> GetConnectionStrings();
        string GetConnectionString(string name);
        int SetConnectionString(string name, string connectionString);
        int DeleteConnectionString(string name);
        IEnumerable<Filter> GetFilters();
        int AddFilter(string filterText);
        int DeleteFilter(string filterText);
        void UpdateFilters(IList<Filter> filters);
        IEnumerable<Column> GetColumns();
        void UpdateColumns(IList<Column> columns);
        bool GetUtc();
        int SetUtc(bool utcValue);
        string GetLastConnectionNickname();
        int SetLastConnectionNickname(string lastConnectionNickname);
        string GetLastExportPath();
        int SetLastExportPath(string lastExportPath);
        bool GetWordwrap();
        int SetWordwrap(bool wordwrap);
    }

    public class Settings : ISettings
    {
        private string _connectionString;
        const string ConnectionStringsTable = "ConnectionStrings";
        const string FiltersTable = "Filters";
        const string UiTable = "UI";
        const string ColumnsTable = "Columns";

        public Settings()
        {
            InitializeDatabase();
        }

        #region ConnectionStrings
        public IList<string> GetConnectionStrings()
        {
            var list = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT Name FROM " + ConnectionStringsTable + " ;";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(DecodeText(reader["Name"] as string));
                        }
                    }
                }
            }
            return list;
        }

        public string GetConnectionString(string name)
        {
            string connectionString = "";
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT ConnectionString FROM " + ConnectionStringsTable + " WHERE Name == @Name;";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", EncodeText(name));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            connectionString = DecodeText(reader["ConnectionString"] as string);
                        }
                    }
                }
            }
            return connectionString;
        }

        public int SetConnectionString(string name, string connectionString)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "INSERT OR REPLACE INTO " + ConnectionStringsTable;
                sql += " (Name, ConnectionString) VALUES (@Name, @ConnectionString);";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", EncodeText(name));
                    cmd.Parameters.AddWithValue("@ConnectionString", EncodeText(connectionString));
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int DeleteConnectionString(string name)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "DELETE FROM " + ConnectionStringsTable + " WHERE Name == @Name;";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", EncodeText(name));
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion ConnectionStrings

        #region Filters

        public IEnumerable<Filter> GetFilters()
        {
            var filters = new List<Filter>();
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT * FROM " + FiltersTable + " ORDER BY Position";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var filter = new Filter
                            {
                                FilterText = DecodeText(reader["FilterText"] as string),
                                Active = reader["Active"] is int && (int)reader["Active"] != 0,
                                Position = reader["Position"] is int ? (int)reader["Position"] : -1
                            };
                            filters.Add(filter);
                        }
                    }
                }
            }
            return filters;
        }

        public int DeleteFilter(string filterText)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "DELETE FROM " + FiltersTable + " WHERE FilterText == @FilterText";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FilterText", EncodeText(filterText));
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int AddFilter(string filterText)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) VALUES (@FilterText)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FilterText", EncodeText(filterText));
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateFilters(IList<Filter> filters)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "UPDATE " + FiltersTable + " SET";
                sql += " Position = @Position, Active = @Active";
                sql += " WHERE FilterText == @FilterText";
                var preparedQuery = new SQLiteCommand(sql, conn);
                preparedQuery.Parameters.Add("@Position", DbType.Int32);
                preparedQuery.Parameters.Add("@Active", DbType.Boolean);
                preparedQuery.Parameters.Add("@FilterText", DbType.String);

                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var filter in filters)
                    {
                        preparedQuery.Parameters["@Position"].Value = filter.Position;
                        preparedQuery.Parameters["@Active"].Value = filter.Active;
                        preparedQuery.Parameters["@FilterText"].Value = EncodeText(filter.FilterText);
                        preparedQuery.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        #endregion Filters

        #region Columns

        public IEnumerable<Column> GetColumns()
        {
            var columns = new List<Column>();
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT * FROM " + ColumnsTable + " ORDER BY Position, Active";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var column = new Column
                            {
                                ColumnName = DecodeText(reader["ColumnName"] as string),
                                Active = reader["Active"] is int && (int)reader["Active"] != 0,
                                Position = reader["Position"] is int ? (int)reader["Position"] : -1
                            };
                            columns.Add(column);
                        }
                    }
                }
            }
            return columns;
        }

        public void UpdateColumns(IList<Column> columns)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "UPDATE " + ColumnsTable + " SET";
                sql += " Position = @Position, Active = @Active";
                sql += " WHERE ColumnName == @ColumnName";
                var preparedQuery = new SQLiteCommand(sql, conn);
                preparedQuery.Parameters.Add("@Position", DbType.Int32);
                preparedQuery.Parameters.Add("@Active", DbType.Boolean);
                preparedQuery.Parameters.Add("@ColumnName", DbType.String);

                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var column in columns)
                    {
                        preparedQuery.Parameters["@Position"].Value = column.Position;
                        preparedQuery.Parameters["@Active"].Value = column.Active;
                        preparedQuery.Parameters["@ColumnName"].Value = EncodeText(column.ColumnName);
                        preparedQuery.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
        }

        #endregion Columns

        #region Utc

        public bool GetUtc()
        {
            var utcValue = false;
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT Value FROM " + UiTable + " WHERE Parameter == 'Utc'";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Boolean.TryParse(reader["Value"] as string, out utcValue);
                        }
                    }
                }
            }
            return utcValue;
        }

        public int SetUtc(bool utcValue)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "INSERT OR REPLACE INTO " + UiTable + " (Parameter,Value) VALUES ('Utc','" + utcValue + "')";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion Utc

        #region Wordwrap
        public bool GetWordwrap()
        {
            bool wordwrapValue = false;
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT Value FROM " + UiTable + " WHERE Parameter == 'Wordwrap'";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Boolean.TryParse(reader["Value"] as string, out wordwrapValue);
                        }
                    }
                }
            }
            return wordwrapValue;
        }

        public int SetWordwrap(bool wordwrapValue)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "INSERT OR REPLACE INTO " + UiTable + " (Parameter,Value) VALUES ('Wordwrap','" + wordwrapValue + "')";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion Wordwrap

        #region LastConnectionNickname

        public string GetLastConnectionNickname()
        {
            string lastConnectionNickname = "";
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT Value FROM " + UiTable + " WHERE Parameter == 'LastConnectionNickname'";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastConnectionNickname = DecodeText(reader["Value"] as string);
                        }
                    }
                }
            }
            return lastConnectionNickname;
        }

        public int SetLastConnectionNickname(string lastConnectionNickname)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "INSERT OR REPLACE INTO " + UiTable + " (Parameter,Value) VALUES ('LastConnectionNickname',@LastConnectionNickname)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@LastConnectionNickname", EncodeText(lastConnectionNickname));
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion LastConnectionNickname

        #region LastExportPath

        public string GetLastExportPath()
        {
            var lastExportPath = "";
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "SELECT Value FROM " + UiTable + " WHERE Parameter == 'LastExportPath'";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastExportPath = DecodeText(reader["Value"] as string);
                        }
                    }
                }
            }
            return lastExportPath;
        }

        public int SetLastExportPath(string lastExportPath)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "INSERT OR REPLACE INTO " + UiTable + " (Parameter,Value) VALUES ('LastExportPath',@LastExportPath)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@LastExportPath", EncodeText(lastExportPath));
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion LastExportPath

        private void InitializeDatabase()
        {
            var applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var myApplicationDataPath = Path.Combine(applicationDataPath, "AzureLogSpelunker");
            Directory.CreateDirectory(myApplicationDataPath);
            var myDataFile = Path.Combine(myApplicationDataPath, "AzureLogSpelunker.db");
            _connectionString = "Data Source=" + myDataFile + ";";

            using (var connection = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "CREATE TABLE IF NOT EXISTS " + ConnectionStringsTable + " ";
                sql += "(Name TEXT PRIMARY KEY , ConnectionString TEXT)";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                sql = "INSERT OR REPLACE INTO " + ConnectionStringsTable + " (Name, ConnectionString) ";
                sql += "VALUES ('" + EncodeText("UseDevelopmentStorage") + "', '" + EncodeText("UseDevelopmentStorage=true") + "');";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                sql = "INSERT OR REPLACE INTO " + ConnectionStringsTable + " (Name, ConnectionString) ";
                sql += "VALUES ('" + EncodeText("Example") + "', '" + EncodeText("DefaultEndpointsProtocol=https;AccountName=foo;AccountKey=barbarbarbarbar==") + "');";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();
            }

            using (var connection = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "CREATE TABLE IF NOT EXISTS " + FiltersTable + " ";
                sql += "(FilterText TEXT PRIMARY KEY, Active INT, Position INT)";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                long count = 0;
                sql = "SELECT COUNT(*) FROM " + FiltersTable;
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    count = (long)cmd.ExecuteScalar();
                }

                if (count == 0)
                {
                    sql = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) ";
                    sql += "VALUES ('" + EncodeText("NOT(Message == 'INFORMATION: <Complaint> Add hard complaint :0 ; TraceSource ''w3wp.exe'' event')") + "')";
                    new SQLiteCommand(sql, connection).ExecuteNonQuery();

                    sql = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) ";
                    sql += "VALUES ('" + EncodeText("NOT(Message LIKE 'INFORMATION: <CASClient> Updated partition table to %')") + "')";
                    new SQLiteCommand(sql, connection).ExecuteNonQuery();

                    sql = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) ";
                    sql += "VALUES ('" + EncodeText("Role == \"somerole\"") + "')";
                    new SQLiteCommand(sql, connection).ExecuteNonQuery();

                    sql = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) ";
                    sql += "VALUES ('" + EncodeText("DeploymentId == \"something\"") + "')";
                    new SQLiteCommand(sql, connection).ExecuteNonQuery();
                }
            }

            using (var connection = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "CREATE TABLE IF NOT EXISTS " + UiTable + " ";
                sql += "(Parameter TEXT PRIMARY KEY, Value TEXT)";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                sql = "INSERT OR IGNORE INTO " + UiTable + " (Parameter, Value) ";
                sql += "VALUES ('LastConnectionNickname', '" + EncodeText("Example") + "')";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();
            }

            using (var connection = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                var sql = "CREATE TABLE IF NOT EXISTS " + ColumnsTable + " ";
                sql += "(ColumnName TEXT PRIMARY KEY, Active INT, Position INT)";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                sql = "INSERT OR IGNORE INTO " + ColumnsTable + " (ColumnName, Active, Position) ";
                sql += "VALUES ('" + EncodeText("Timestamp") + "', 1, 0) ";
                sql += ", ('" + EncodeText("DeploymentId") + "', 1, 1) ";
                sql += ", ('" + EncodeText("Role") + "', 1, 2) ";
                sql += ", ('" + EncodeText("RoleInstance") + "', 1, 3) ";
                sql += ", ('" + EncodeText("Message") + "', 1, 4) ";
                new SQLiteCommand(sql, connection).ExecuteNonQuery();

                var columnInfo = Utility.GetProperties<LogEntity>();
                foreach (var info in columnInfo)
                {
                    sql = "INSERT OR IGNORE INTO " + ColumnsTable + " (ColumnName, Position) ";
                    sql += "VALUES ('" + EncodeText(info.Name) + "', 1000) ";
                    new SQLiteCommand(sql, connection).ExecuteNonQuery();
                }
            }

        }

        private string EncodeText(string plainText)
        {
            return Uri.EscapeDataString(plainText);
        }

        private string DecodeText(string encoded)
        {
            return Uri.UnescapeDataString(encoded);
        }

        /*
        private SQLiteDataReader Query(string sql)
        {
            using (var conn = new SQLiteConnection(_ConnectionString).OpenAndReturn())
            {
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return cmd.ExecuteReader();
                }
            }
        }

        private int NonQuery(string sql)
        {
            using (var conn = new SQLiteConnection(_ConnectionString).OpenAndReturn())
            {
                return new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }
        */
    }
}
