using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Api.Launch.Model
{
    /// <summary>
    /// Describes modes for launches.
    /// </summary>
    public enum LaunchMode
    {
        [DataMember(Name = "DEFAULT")]
        Default,
        [DataMember(Name = "DEBUG")]
        Debug
    }
}
