using System.Runtime.Serialization;

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
