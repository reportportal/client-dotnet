using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents the response received when a launch is created.
    /// </summary>
    public class LaunchCreatedResponse
    {
        /// <summary>
        /// Gets or sets the UUID of the launch.
        /// </summary>
        [JsonPropertyName("id")]
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the number of the launch.
        /// </summary>
        public long Number { get; set; }
    }
}
