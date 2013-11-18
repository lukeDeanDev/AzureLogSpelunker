using System.Collections.Generic;

namespace AzureLogSpelunker.Models
{
    public class AzureQueryModel
    {
        public string ConnectionString { get; set; }
        public string BeginPartitionKey { get; set; }
        public string EndPartitionKey { get; set; }
        public IEnumerable<LogEntity> ResultSet { get; set; }
    }
}
