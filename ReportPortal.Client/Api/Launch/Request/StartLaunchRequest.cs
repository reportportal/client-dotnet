using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReportPortal.Client.Api.Launch.Model;
using ReportPortal.Client.Converter;

namespace ReportPortal.Client.Api.Launch.Request
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
        public string Name
        {
            get => _name;
            set => _name = StringTrimmer.Trim(value, 256);
        }

        /// <summary>
        /// Description of launch.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        [DataMember(Name = "mode")]
        public string ModeString
        {
            get => EnumConverter.ConvertFrom(Mode);
            set => Mode = EnumConverter.ConvertTo<LaunchMode>(value);
        }

        public LaunchMode Mode { get; set; } = LaunchMode.Default;

        /// <summary>
        /// Date time when the launch is executed.
        /// </summary>
        [DataMember(Name = "start_time")]
        public string StartTimeString { get; set; }

        public DateTime StartTime
        {
            get => DateTimeConverter.ConvertTo(StartTimeString);
            set => StartTimeString = DateTimeConverter.ConvertFrom(value);
        }

        /// <summary>
        /// Mark the launch with tags.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }
    }
}
