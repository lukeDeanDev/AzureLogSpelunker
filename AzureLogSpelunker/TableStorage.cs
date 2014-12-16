using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AzureLogSpelunker.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Data;

namespace AzureLogSpelunker
{
    public class TableStorage
    {
        public static string UtcDateTimeToPartitionKey(DateTime dateTime)
        {
            var dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);
            var retVal = "0" + dt.Ticks.ToString(CultureInfo.InvariantCulture);
            return retVal;
        }

        public static IEnumerable<string> GetTableNames(string connectionString)
        {
            var storageAccount = connectionString == "UseDevelopmentStorage=true" ? CloudStorageAccount.DevelopmentStorageAccount : CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            return tableClient.ListTables().Select(table => table.Name).ToList();
        }

        public static IEnumerable<LogEntity> Go(string connectionString, string tableName, string beginPartitionKey, string endPartitionKey)
        {
            var storageAccount = connectionString == "UseDevelopmentStorage=true" ? CloudStorageAccount.DevelopmentStorageAccount : CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference(tableName);
            if (!table.Exists())
                throw new ApplicationException(string.Format("{0} does not exist", tableName));


            var query = new TableQuery<LogEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThanOrEqual, beginPartitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.LessThanOrEqual, endPartitionKey)
                )
            );

            var resultSet = table.ExecuteQuery(query);

            return resultSet;
        }

        public static DataTable MakeDataTable<T>()
        {
            var dataTable = new DataTable();
            var properties = Utility.GetProperties<T>();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, SqlitifyType(info.PropertyType)));
            }
            return dataTable;
        }

        public static void FillDataTable<T>(DataTable dataTable, IEnumerable<T> items)
        {
            var properties = Utility.GetProperties<T>();
            foreach (var item in items)
            {
                var row = dataTable.NewRow();
                foreach (var info in properties)
                {
                    row[info.Name] = info.GetValue(item);
                }
                dataTable.Rows.Add(row);
            }
        }

        private static Type SqlitifyType(Type inType)
        {
            //SQLite doesn't do DateTimeOffset, so we'll just make those strings (and remember not to sort by them)
            if (inType == typeof(DateTimeOffset))
                return typeof(String);
            return inType;
        }
    }
}
