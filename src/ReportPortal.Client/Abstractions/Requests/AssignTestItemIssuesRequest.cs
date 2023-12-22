using ReportPortal.Client.Abstractions.Responses;
using System.Collections.Generic;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for assigning issues to test items.
    /// </summary>
    public class AssignTestItemIssuesRequest
    {
        /// <summary>
        /// List of test items and their issues.
        /// </summary>
        public List<TestItemIssueUpdate> Issues { get; set; }
    }

    public class TestItemIssueUpdate
    {
        /// <summary>
        /// A issue of test item.
        /// </summary>
        public Issue Issue { get; set; }

        /// <summary>
        /// ID of test item to assign the issue.
        /// </summary>
        public long TestItemId { get; set; }
    }
}
