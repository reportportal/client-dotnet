using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class LaunchFinishedResponse
    {
        [JsonPropertyName("id")]
        public string Uuid { get; set; }

        public string Link { get; set; }
    }
}
