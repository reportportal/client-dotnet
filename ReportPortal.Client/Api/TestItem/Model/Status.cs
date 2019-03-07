using System.Runtime.Serialization;

namespace ReportPortal.Client.Api.TestItem.Model
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
        Interrupted
    }
}
