using System;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class LogItem
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "time")]
        public string TimeString { get; set; }

        public DateTime Time
        {
            get
            {
                return DateTimeConverter.ConvertTo(TimeString);
            }
            set
            {
                TimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        [DataMember(Name = "message")]
        public string Text { get; set; }

        [DataMember(Name = "level")]
        public string LevelString { get; set; }

        public LogLevel Level { get { return EnumConverter.ConvertTo<LogLevel>(LevelString); } set { LevelString = EnumConverter.ConvertFrom(value); } }

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

    [DataContract]
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

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public byte[] Data { get; set; }

        [IgnoreDataMember]
        public string MimeType { get; set; }
    }
}
