using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes modes for launches.
    /// </summary>
    public enum LaunchMode
    {
        /// <summary>
        /// The default launch mode.
        /// </summary>
        [JsonPropertyName("DEFAULT")]
        Default,

        /// <summary>
        /// The debug launch mode.
        /// </summary>
        [JsonPropertyName("DEBUG")]
        Debug
    }
}
