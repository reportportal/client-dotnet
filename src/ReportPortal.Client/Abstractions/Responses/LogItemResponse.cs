using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a response object for a log item.
    /// </summary>
    public class LogItemResponse
    {
        /// <summary>
        /// Gets or sets the ID of the log item.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the UUID of the log item.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the ID of the test item associated with the log item.
        /// </summary>
        [JsonPropertyName("itemId")]
        public long TestItemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the launch associated with the log item.
        /// </summary>
        public long LaunchId { get; set; }

        /// <summary>
        /// Gets or sets the time when the log item was created.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the text message of the log item.
        /// </summary>
        [JsonPropertyName("message")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the log level of the log item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<LogLevel>))]
        public LogLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the binary content of the log item.
        /// </summary>
        [JsonPropertyName("binaryContent")]
        public BinaryContent Content { get; set; }
    }

    /// <summary>
    /// Represents the binary content of a log item.
    /// </summary>
    public class BinaryContent
    {
        /// <summary>
        /// Gets or sets the content type of the binary content.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the ID of the binary content.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail ID of the binary content.
        /// </summary>
        public string ThumbnailId { get; set; }
    }
}
