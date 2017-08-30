using System.Collections.Generic;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request to finish specified launch.
    /// </summary>
    public class UpdateLaunchRequest
    {
        /// <summary>
        /// Update tags for launch.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of launch.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Specify whether the launch is executed under debugging.
        /// </summary>
        public LaunchMode Mode { get; set; }
    }
}
