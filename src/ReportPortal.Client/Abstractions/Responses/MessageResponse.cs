using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a response containing a message.
    /// </summary>
    public class MessageResponse
    {
        /// <summary>
        /// Gets or sets the message information.
        /// </summary>
        [JsonPropertyName("message")]
        public string Info { get; set; }
    }
}
