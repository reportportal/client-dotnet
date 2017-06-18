using System;
using ReportPortal.Client.Converters;
using Newtonsoft.Json;

namespace ReportPortal.Client.Models
{
    public class LogItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Time { get; set; }

        [JsonProperty("message")]
        public string Text { get; set; }

        [JsonProperty("level")]
        [JsonConverter(typeof(LogLevelConverter))]
        public LogLevel Level { get; set; }

        [JsonProperty("binary_content")]
        public BinaryContent Content { get; set; }
    }

    public class BinaryContent
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        public string Id { get; set; }

        [JsonProperty("thumbnail_id")]
        public string ThumbnailId { get; set; }
    }

    public class Attach
    {
        public Attach()
        {

        }

        public Attach(string name, string mimeType, byte[] data)
        {
            Name = name;
            MimeType = mimeType;
            Data = data;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public byte[] Data { get; set; }

        [JsonIgnore]
        public string MimeType { get; set; }
    }
}
