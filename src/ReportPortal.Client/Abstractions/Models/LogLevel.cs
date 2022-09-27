using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes levels for log items.
    /// </summary>
    public enum LogLevel
    {
        [JsonPropertyName("TRACE")]
        Trace,
        [JsonPropertyName("DEBUG")]
        Debug,
        [JsonPropertyName("INFO")]
        Info,
        [JsonPropertyName("WARN")]
        Warning,
        [JsonPropertyName("ERROR")]
        Error,
        [JsonPropertyName("FATAL")]
        Fatal
    }
}
