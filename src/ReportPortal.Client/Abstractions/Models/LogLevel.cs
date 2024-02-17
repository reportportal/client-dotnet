using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes levels for log items.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Represents the TRACE log level.
        /// </summary>
        [JsonPropertyName("TRACE")]
        Trace,

        /// <summary>
        /// Represents the DEBUG log level.
        /// </summary>
        [JsonPropertyName("DEBUG")]
        Debug,

        /// <summary>
        /// Represents the INFO log level.
        /// </summary>
        [JsonPropertyName("INFO")]
        Info,

        /// <summary>
        /// Represents the WARNING log level.
        /// </summary>
        [JsonPropertyName("WARN")]
        Warning,

        /// <summary>
        /// Represents the ERROR log level.
        /// </summary>
        [JsonPropertyName("ERROR")]
        Error,

        /// <summary>
        /// Represents the FATAL log level.
        /// </summary>
        [JsonPropertyName("FATAL")]
        Fatal
    }
}
