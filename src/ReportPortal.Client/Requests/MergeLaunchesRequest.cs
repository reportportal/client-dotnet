using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new launch.
    /// </summary>
    [DataContract]
    public class MergeLaunchesRequest
    {
        /// <summary>
        /// A short name of launch.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        [DataMember(Name = "mode", EmitDefaultValue = true)]
        public string ModeString { get { return EnumConverter.ConvertFrom(Mode); } set { Mode = EnumConverter.ConvertTo<LaunchMode>(value); } }

        public LaunchMode Mode { get; set; }

        /// <summary>
        /// Date time when the launch is executed.
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
        /// Date time when the launch is finished.
        /// </summary>
        [DataMember(Name = "end_time")]
        public string EndTimeString { get; set; }

        public DateTime EndTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(EndTimeString);
            }
            set
            {
                EndTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        /// <summary>
        /// Tags for merged launch.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Tags for merged launch.
        /// </summary>
        [DataMember(Name = "launches")]
        public List<string> Launches { get; set; }

        /// <summary>
        /// Type of launches merge.
        /// </summary>
        [DataMember(Name = "merge_type")]
        public string MergeType { get; set; }
    }
}
