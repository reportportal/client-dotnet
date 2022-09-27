using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class LogItemsCreatedResponse
    {
        [JsonPropertyName("responses")]
        public IList<LogItemCreatedResponse> LogItems { get; set; }
    }

    public class LogItemCreatedResponse
    {
        [JsonPropertyName("id")]
        public string Uuid { get; set; }
    }
}
