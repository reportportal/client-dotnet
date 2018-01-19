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
    public class StartLaunchRequest
    {
        private string _name;

        /// <summary>
        /// A short name of launch.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get { return _name; } set { _name = StringTrimmer.Trim(value, 256); } }

        /// <summary>
        /// Description of launch.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        [DataMember(Name = "mode")]
        public string ModeString { get { return EnumConverter.ConvertFrom(Mode); } set { Mode = EnumConverter.ConvertTo<LaunchMode>(value); } }

        public LaunchMode Mode = LaunchMode.Default;

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
        /// Mark the launch with tags.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }
    }
}
