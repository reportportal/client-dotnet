﻿using System;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request for logging messages into Report Portal.
    /// </summary>
    [DataContract]
    public class AddLogItemRequest
    {
        /// <summary>
        /// ID of test item to add new logs.
        /// </summary>
        [DataMember(Name = "item_id")]
        public string TestItemId { get; set; }

        /// <summary>
        /// Date time of log item.
        /// </summary>
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

        /// <summary>
        /// A level of log item.
        /// </summary>
        [DataMember(Name = "level")]
        public string LevelString { get { return EnumConverter.ConvertFrom(Level); } set { Level = EnumConverter.ConvertTo<LogLevel>(value); } }

        public LogLevel Level { get; set; } = LogLevel.Info;

        /// <summary>
        /// Message of log item.
        /// </summary>
        [DataMember(Name = "message")]
        public string Text { get; set; }

        /// <summary>
        /// Specify an attachment of log item.
        /// </summary>
        [DataMember(Name = "file")]
        public Attach Attach { get; set; }
    }
}
