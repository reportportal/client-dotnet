using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Filtering
{
    public enum SortDirection
    {
        [JsonPropertyName("ASC")]
        Ascending,
        [JsonPropertyName("DESC")]
        Descending
    }
    public class Sorting
    {
        public Sorting(List<string> byFields, SortDirection direction = SortDirection.Ascending)
        {
            Fields = byFields;
            Direction = direction;
        }

        public List<string> Fields { get; set; }
        public SortDirection Direction { get; set; }
    }
}
