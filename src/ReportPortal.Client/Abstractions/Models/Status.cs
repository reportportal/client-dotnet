using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes statuses of tests items.
    /// </summary>
    public enum Status
    {
        [JsonPropertyName("IN_PROGRESS")]
        InProgress,
        [JsonPropertyName("PASSED")]
        Passed,
        [JsonPropertyName("FAILED")]
        Failed,
        [JsonPropertyName("SKIPPED")]
        Skipped,
        [JsonPropertyName("INTERRUPTED")]
        Interrupted,
        [JsonPropertyName("CANCELLED")]
        Cancelled,
        [JsonPropertyName("STOPPED")]
        Stopped,
        [JsonPropertyName("INFO")]
        Info,
        [JsonPropertyName("WARN")]
        Warn
    }
}
