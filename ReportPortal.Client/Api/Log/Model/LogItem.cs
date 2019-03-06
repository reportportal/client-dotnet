using System;
using ReportPortal.Client.Converter;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ReportPortal.Client.Api.Log.Model
{
    [DataContract]
    public class LogItem
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

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

        [DataMember(Name = "binary_content")]
        public BinaryContent Content { get; set; }
    }

    [DataContract]
    public class BinaryContent
    {
        [DataMember(Name = "content_type")]
        public string ContentType { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "thumbnail_id")]
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
            Data = new ReadOnlyCollection<byte>(data);
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [IgnoreDataMember]
        public ReadOnlyCollection<byte> Data { get; }

        [IgnoreDataMember]
        public string MimeType { get; set; }
    }

    [DataContract]
    public class Responses
    {
        [DataMember(Name = "responses")]
        public List<LogItem> LogItems { get; set; }
    }
}
