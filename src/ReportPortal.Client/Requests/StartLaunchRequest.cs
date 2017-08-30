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
    public class StartLaunchRequest
    {
        /// <summary>
        /// A short name of launch.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
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
        /// Mark the launch with tags.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public List<string> Tags { get; set; }
    }
}
