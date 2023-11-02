using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public class ItemAttribute
    {
        public string Key { get; set; }

        public string Value { get; set; }

        [JsonPropertyName("system")]
        public bool IsSystem { get; set; }
    }
}
