using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Represents the type of filter for a user.
    /// </summary>
    public enum UserFilterType
    {
        /// <summary>
        /// Represents a filter for launches.
        /// </summary>
        [JsonPropertyName("launch")]
        Launch,

        /// <summary>
        /// Represents a filter for test items.
        /// </summary>
        [JsonPropertyName("testitem")]
        TestItem,

        /// <summary>
        /// Represents a filter for logs.
        /// </summary>
        [JsonPropertyName("log")]
        Log
    }
}
