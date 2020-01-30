using System.Collections.Generic;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for assigning issues to test items.
    /// </summary>
    [DataContract]
    public class AssignTestItemIssuesRequest
    {
        /// <summary>
        /// List of test items and their issues.
        /// </summary>
        [DataMember(Name = "issues")]
        public List<TestItemIssueUpdate> Issues { get; set; }
    }

    [DataContract]
    public class TestItemIssueUpdate
    {
        /// <summary>
        /// A issue of test item.
        /// </summary>
        [DataMember(Name = "issue")]
        public Issue Issue { get; set; }

        /// <summary>
        /// ID of test item to assign the issue.
        /// </summary>
        [DataMember(Name = "testItemId")]
        public long TestItemId { get; set; }
    }
}
