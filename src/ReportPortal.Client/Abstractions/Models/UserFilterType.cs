using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public enum UserFilterType
    {
        [JsonPropertyName("launch")]
        Launch,
        [JsonPropertyName("testitem")]
        TestItem,
        [JsonPropertyName("log")]
        Log
    }
}
