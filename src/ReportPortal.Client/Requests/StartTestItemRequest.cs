using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new test item in progress state.
    /// </summary>
    [DataContract]
    public class StartTestItemRequest
    {
        /// <summary>
        /// ID of parent launch to create new test item.
        /// </summary>
        [DataMember(Name = "launch_id")]
        public string LaunchId { get; set; }

        /// <summary>
        /// A short name of test item.
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// A long description of test item.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// Date time when new test item is created.
        /// </summary>
        [DataMember(Name = "start_time")]
        public string StartTimeString { get; set; }

        public DateTime StartTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(StartTimeString);
            }
            set
            {
                StartTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        /// <summary>
        /// A type of test item.
        /// </summary>
        public TestItemType Type { get; set; }

        /// <summary>
        /// A list of tags.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
