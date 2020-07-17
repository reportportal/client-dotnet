using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes statuses of tests items.
    /// </summary>
    public enum Status
    {
        [DataMember(Name = "IN_PROGRESS")]
        InProgress,
        [DataMember(Name = "PASSED")]
        Passed,
        [DataMember(Name = "FAILED")]
        Failed,
        [DataMember(Name = "SKIPPED")]
        Skipped,
        [DataMember(Name = "INTERRUPTED")]
        Interrupted,
        [DataMember(Name = "CANCELLED")]
        Cancelled,
        [DataMember(Name = "STOPPED")]
        Stopped,
        [DataMember(Name = "INFO")]
        Info,
        [DataMember(Name = "WARN")]
        Warn
    }
}
