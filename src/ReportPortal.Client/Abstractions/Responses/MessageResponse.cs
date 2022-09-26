using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class MessageResponse
    {
        [JsonPropertyName("message")]
        public string Info { get; set; }
    }
}
