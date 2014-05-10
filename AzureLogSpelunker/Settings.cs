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
        string MyApplicationDataPath { get; }
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
        string GetFetchTo();
        int SetFetchTo(string choice);
    }

    public class Settings : ISettings
    {
        private readonly string _connectionString;
        private readonly string _myApplicationDataPath;
        private const string ConnectionStringsTable = "ConnectionStrings";
        private const string FiltersTable = "Filters";
        private const string UiTable = "UI";
        private const string ColumnsTable = "Columns";

        public Settings()
        {
            var applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _myApplicationDataPath = Path.Combine(applicationDataPath, "AzureLogSpelunker");
            Directory.CreateDirectory(_myApplicationDataPath);
            var myDataFile = Path.Combine(_myApplicationDataPath, "AzureLogSpelunker.db");
            _connectionString = "Data Source=" + myDataFile + ";";

            using (var connection = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                InitializeConnectionStringsTable(connection);
                InitializeFiltersTable(connection);
                InitializeUiTable(connection);
                InitializeColumnsTable(connection);
            }
        }

        public string MyApplicationDataPath
        {
            get { return _myApplicationDataPath; }
        }

        #region ConnectionStrings
        public IList<string> GetConnectionStrings()
        {
            var list = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                const string sql = "SELECT Name FROM " + ConnectionStringsTable + " ;";
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
                const string sql = "SELECT ConnectionString FROM " + ConnectionStringsTable + " WHERE Name == @Name;";
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
                const string sql = "INSERT OR REPLACE INTO " + ConnectionStringsTable + " (Name, ConnectionString) VALUES (@Name, @ConnectionString);";
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
                const string sql = "DELETE FROM " + ConnectionStringsTable + " WHERE Name == @Name;";
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
                const string sql = "SELECT * FROM " + FiltersTable + " ORDER BY Position";
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
                const string sql = "DELETE FROM " + FiltersTable + " WHERE FilterText == @FilterText";
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
                const string sql = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) VALUES (@FilterText)";
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
                const string sql = "UPDATE " + FiltersTable + " SET Position = @Position, Active = @Active WHERE FilterText == @FilterText";
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
                const string sql = "SELECT * FROM " + ColumnsTable + " ORDER BY Position, Active";
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
                const string sql = "UPDATE " + ColumnsTable + " SET Position = @Position, Active = @Active WHERE ColumnName == @ColumnName";
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

        #region Ui Parameters

        public bool GetUtc()
        {
            bool utcValue;
            Boolean.TryParse(GetUiParameter("Utc"), out utcValue);
            return utcValue;
        }

        public int SetUtc(bool utcValue)
        {
            return SetUiParameter("Utc", utcValue.ToString());
        }

        public bool GetWordwrap()
        {
            bool wordwrapValue;
            Boolean.TryParse(GetUiParameter("Wordwrap"), out wordwrapValue);
            return wordwrapValue;
        }

        public int SetWordwrap(bool wordwrapValue)
        {
            return SetUiParameter("Wordwrap", wordwrapValue.ToString());
        }

        public string GetLastConnectionNickname()
        {
            return GetUiParameter("LastConnectionNickname");
        }

        public int SetLastConnectionNickname(string lastConnectionNickname)
        {
            return SetUiParameter("LastConnectionNickname", lastConnectionNickname);
        }

        public string GetLastExportPath()
        {
            return GetUiParameter("LastExportPath");
        }

        public int SetLastExportPath(string lastExportPath)
        {
            return SetUiParameter("LastExportPath", lastExportPath);
        }

        public string GetFetchTo()
        {
            return GetUiParameter("FetchTo");
        }

        public int SetFetchTo(string choice)
        {
            return SetUiParameter("FetchTo", choice);
        }

        private string GetUiParameter(string parameter)
        {
            var returnValue = "";
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                const string sql = "SELECT Value FROM " + UiTable + " WHERE Parameter == @Parameter";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Parameter", parameter);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnValue = DecodeText(reader["Value"] as string);
                        }
                    }
                }
            }
            return returnValue;
        }

        private int SetUiParameter(string parameter, string value)
        {
            using (var conn = new SQLiteConnection(_connectionString).OpenAndReturn())
            {
                const string sql = "INSERT OR REPLACE INTO " + UiTable + " (Parameter,Value) VALUES (@Parameter, @Value)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Parameter", parameter);
                    cmd.Parameters.AddWithValue("@Value", EncodeText(value));
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion Ui Parameters

        #region Initialization
        private static void InitializeConnectionStringsTable(SQLiteConnection connection)
        {
            const string sqlCreate = "CREATE TABLE IF NOT EXISTS " + ConnectionStringsTable + " (Name TEXT PRIMARY KEY , ConnectionString TEXT)";
            new SQLiteCommand(sqlCreate, connection).ExecuteNonQuery();

            const string sqlInsert = "INSERT OR REPLACE INTO " + ConnectionStringsTable + " (Name, ConnectionString) VALUES (@Name, @ConnectionString);";
            using (var cmd = new SQLiteCommand(sqlInsert, connection))
            {
                cmd.Parameters.AddWithValue("@Name", EncodeText("UseDevelopmentStorage"));
                cmd.Parameters.AddWithValue("@ConnectionString", EncodeText("UseDevelopmentStorage=true"));
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@Name", EncodeText("Example"));
                cmd.Parameters.AddWithValue("@ConnectionString", EncodeText("DefaultEndpointsProtocol=https;AccountName=foo;AccountKey=barbarbarbarbar=="));
                cmd.ExecuteNonQuery();
            }
        }

        private static void InitializeFiltersTable(SQLiteConnection connection)
        {
            const string sqlCreate = "CREATE TABLE IF NOT EXISTS " + FiltersTable + " (FilterText TEXT PRIMARY KEY, Active INT, Position INT)";
            new SQLiteCommand(sqlCreate, connection).ExecuteNonQuery();

            long count;
            const string sqlCount = "SELECT COUNT(*) FROM " + FiltersTable;
            using (var cmd = new SQLiteCommand(sqlCount, connection))
            {
                count = (long)cmd.ExecuteScalar();
            }

            if (count == 0)
            {
                const string sqlInsert = "INSERT OR IGNORE INTO " + FiltersTable + " (FilterText) VALUES (@FilterText)";
                using (var cmd = new SQLiteCommand(sqlInsert, connection))
                {
                    cmd.Parameters.AddWithValue("@FilterText", EncodeText("NOT(Message == 'INFORMATION: <Complaint> Add hard complaint :0 ; TraceSource ''w3wp.exe'' event')"));
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.AddWithValue("@FilterText", EncodeText("NOT(Message LIKE 'INFORMATION: <CASClient> Updated partition table to %')"));
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.AddWithValue("@FilterText", EncodeText("Role == \"somerole\""));
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.AddWithValue("@FilterText", EncodeText("DeploymentId == \"something\""));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void InitializeUiTable(SQLiteConnection connection)
        {
            const string sqlCreate = "CREATE TABLE IF NOT EXISTS " + UiTable + " (Parameter TEXT PRIMARY KEY, Value TEXT)";
            new SQLiteCommand(sqlCreate, connection).ExecuteNonQuery();

            const string sqlInsert = "INSERT OR IGNORE INTO " + UiTable + " (Parameter, Value) VALUES (@Parameter, @Value)";
            using (var cmd = new SQLiteCommand(sqlInsert, connection))
            {
                cmd.Parameters.AddWithValue("@Parameter", "LastConnectionNickname");
                cmd.Parameters.AddWithValue("@Value", EncodeText("Example"));
                cmd.ExecuteNonQuery();
            }
        }

        private static void InitializeColumnsTable(SQLiteConnection connection)
        {
            const string sqlCreate = "CREATE TABLE IF NOT EXISTS " + ColumnsTable + " (ColumnName TEXT PRIMARY KEY, Active INT, Position INT)";
            new SQLiteCommand(sqlCreate, connection).ExecuteNonQuery();

            const string sqlInsert = "INSERT OR IGNORE INTO " + ColumnsTable + " (ColumnName, Active, Position) VALUES (@ColumnName, @Active, @Position) ";
            using (var cmd = new SQLiteCommand(sqlInsert, connection))
            {
                cmd.Parameters.AddWithValue("@ColumnName", EncodeText("Timestamp"));
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Position", 0);
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@ColumnName", EncodeText("DeploymentId"));
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Position", 1);
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@ColumnName", EncodeText("Role"));
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Position", 2);
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@ColumnName", EncodeText("RoleInstance"));
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Position", 3);
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@ColumnName", EncodeText("Message"));
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Position", 4);
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("@ColumnName", EncodeText("RowId")); //The SQLite internal RowId
                cmd.Parameters.AddWithValue("@Active", 0);
                cmd.Parameters.AddWithValue("@Position", 1000);
                cmd.ExecuteNonQuery();

                var columnInfo = Utility.GetProperties<LogEntity>();
                foreach (var info in columnInfo)
                {
                    cmd.Parameters.AddWithValue("@ColumnName", EncodeText(info.Name));
                    cmd.Parameters.AddWithValue("@Active", 0);
                    cmd.Parameters.AddWithValue("@Position", 1000);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion Initialization

        private static string EncodeText(string plainText)
        {
            return Uri.EscapeDataString(plainText);
        }

        private static string DecodeText(string encoded)
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
