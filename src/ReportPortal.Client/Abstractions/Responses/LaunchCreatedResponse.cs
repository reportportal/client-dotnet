using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class LaunchCreatedResponse
    {
        [JsonPropertyName("id")]
        public string Uuid { get; set; }

        public long Number { get; set; }
    }
}
