using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes statuses of tests items.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Test item is in progress.
        /// </summary>
        [JsonPropertyName("IN_PROGRESS")]
        InProgress,

        /// <summary>
        /// Test item has passed.
        /// </summary>
        [JsonPropertyName("PASSED")]
        Passed,

        /// <summary>
        /// Test item has failed.
        /// </summary>
        [JsonPropertyName("FAILED")]
        Failed,

        /// <summary>
        /// Test item has been skipped.
        /// </summary>
        [JsonPropertyName("SKIPPED")]
        Skipped,

        /// <summary>
        /// Test item has been interrupted.
        /// </summary>
        [JsonPropertyName("INTERRUPTED")]
        Interrupted,

        /// <summary>
        /// Test item has been cancelled.
        /// </summary>
        [JsonPropertyName("CANCELLED")]
        Cancelled,

        /// <summary>
        /// Test item has been stopped.
        /// </summary>
        [JsonPropertyName("STOPPED")]
        Stopped,

        /// <summary>
        /// Test item provides information.
        /// </summary>
        [JsonPropertyName("INFO")]
        Info,

        /// <summary>
        /// Test item has a warning.
        /// </summary>
        [JsonPropertyName("WARN")]
        Warn
    }
}
