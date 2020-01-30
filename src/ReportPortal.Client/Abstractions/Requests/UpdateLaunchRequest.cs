using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish specified launch.
    /// </summary>
    [DataContract]
    public class UpdateLaunchRequest
    {
        /// <summary>
        /// Update tags for launch.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

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
    }
}
