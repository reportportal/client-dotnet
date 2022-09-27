using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes modes for launches.
    /// </summary>
    public enum LaunchMode
    {
        [JsonPropertyName("DEFAULT")]
        Default,
        [JsonPropertyName("DEBUG")]
        Debug
    }
}
