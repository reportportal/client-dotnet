using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class LogItemResponse
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "itemId")]
        public long TestItemId { get; set; }

        [DataMember(Name = "launchId")]
        public long LaunchId { get; set; }

        [DataMember(Name = "time")]
        private string TimeString { get; set; }

        public DateTime Time => DateTimeConverter.ConvertTo(TimeString);

        [DataMember(Name = "message")]
        public string Text { get; set; }

        [DataMember(Name = "level")]
        private string LevelString { get; set; }

        public LogLevel Level => EnumConverter.ConvertTo<LogLevel>(LevelString);

        [DataMember(Name = "binaryContent")]
        public BinaryContent Content { get; set; }
    }

    [DataContract]
    public class BinaryContent
    {
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "thumbnailId")]
        public string ThumbnailId { get; set; }
    }
}
