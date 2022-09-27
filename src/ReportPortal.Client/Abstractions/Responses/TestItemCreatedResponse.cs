using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class TestItemCreatedResponse
    {
        [JsonPropertyName("id")]
        public string Uuid { get; set; }
    }
}
