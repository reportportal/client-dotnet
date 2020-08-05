using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
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

        public LaunchMode Mode { get; set; } = LaunchMode.Default;

        /// <summary>
        /// Date time when the launch is executed.
        /// </summary>
        [DataMember(Name = "startTime")]
        public string StartTimeString { get; set; } = DateTimeConverter.ConvertFrom(DateTime.UtcNow);

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
        [Obsolete("Use Attributes instead of Tags.")]
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Don't start new launch and use some existing.
        /// </summary>
        [DataMember(Name = "rerun")]
        public bool IsRerun { get; set; }

        /// <summary>
        /// Don't start new launch and use some existing with specific launch UUID.
        /// </summary>
        [DataMember(Name = "rerunOf")]
        public string RerunOfLaunchUuid { get; set; }

        /// <summary>
        /// Launch attributes.
        /// </summary>
        [DataMember(Name = "attributes")]
        public IList<ItemAttribute> Attributes { get; set; }
    }
}
