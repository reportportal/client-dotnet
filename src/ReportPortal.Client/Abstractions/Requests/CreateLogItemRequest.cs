using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for logging messages into Report Portal.
    /// </summary>
    public class CreateLogItemRequest
    {
        /// <summary>
        /// ID of test item to add new logs.
        /// </summary>
        [JsonPropertyName("itemUuid")]
        public string TestItemUuid { get; set; }

        /// <summary>
        /// Log item belongs to launch instead of test item if test item id is null.
        /// </summary>
        public string LaunchUuid { get; set; }

        /// <summary>
        /// Date time of log item.
        /// </summary>
        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime Time { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// A level of log item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<LogLevel>))]
        public LogLevel Level { get; set; } = LogLevel.Info;

        /// <summary>
        /// Message of log item.
        /// </summary>
        [JsonPropertyName("message")]
        public string Text { get; set; }

        /// <summary>
        /// Specify an attachment of log item.
        /// </summary>
        [JsonPropertyName("file")]
        public LogItemAttach Attach { get; set; }
    }

    /// <summary>
    /// Represents an attachment for a log item.
    /// </summary>
    public class LogItemAttach
    {
        // empty ctor for json serialization
        public LogItemAttach()
        {

        }

        /// <summary>
        /// Initializes a new instance of the LogItemAttach class with the specified MIME type and data.
        /// </summary>
        /// <param name="mimeType">The MIME type of the attachment.</param>
        /// <param name="data">The data of the attachment.</param>
        public LogItemAttach(string mimeType, byte[] data)
        {
            MimeType = mimeType;
            Data = data;
        }

        /// <summary>
        /// Gets or sets the name of the attachment.
        /// </summary>
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [JsonIgnore]
        public byte[] Data { get; set; }

        [JsonIgnore]
        public string MimeType { get; set; }
    }
}
