using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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
