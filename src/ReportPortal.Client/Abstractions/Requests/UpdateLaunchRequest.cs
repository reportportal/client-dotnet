using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
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
        [Obsolete("Use Attributes instead of Tags.")]
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Update attributes for launch.
        /// </summary>
        [DataMember(Name = "attributes", EmitDefaultValue = true)]
        public List<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "mode", EmitDefaultValue = true)]
        private string _modeString;

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        public LaunchMode? Mode
        {
            get
            {
                return EnumConverter.ConvertTo<LaunchMode>(_modeString);
            }
            set
            {
                _modeString = EnumConverter.ConvertFrom(value);
            }
        }
    }
}
