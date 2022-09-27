using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class LogItemResponse
    {
        public long Id { get; set; }

        public string Uuid { get; set; }

        [JsonPropertyName("itemId")]
        public long TestItemId { get; set; }

        public long LaunchId { get; set; }

        [JsonConverter(typeof(DateTimeUnixEpochConverter))]
        public DateTime Time { get; set; }

        [JsonPropertyName("message")]
        public string Text { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<LogLevel>))]
        public LogLevel Level { get; set; }

        [JsonPropertyName("binaryContent")]
        public BinaryContent Content { get; set; }
    }

    public class BinaryContent
    {
        public string ContentType { get; set; }

        public string Id { get; set; }

        public string ThumbnailId { get; set; }
    }
}
