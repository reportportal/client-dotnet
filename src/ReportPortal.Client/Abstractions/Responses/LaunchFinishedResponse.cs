using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents the response for a finished launch.
    /// </summary>
    public class LaunchFinishedResponse
    {
        /// <summary>
        /// Gets or sets the UUID of the launch.
        /// </summary>
        [JsonPropertyName("id")]
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the link associated with the launch.
        /// </summary>
        public string Link { get; set; }
    }
}
