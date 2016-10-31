using System.Collections.Generic;
using ReportPortal.Client.Models;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request for assigning issues to test items.
    /// </summary>
    public class AssignTestItemIssuesRequest
    {
        /// <summary>
        /// List of test items and their issues.
        /// </summary>
        [JsonProperty("issues")]
        public List<TestItemIssueUpdate> Issues { get; set; }
    }

    public class TestItemIssueUpdate
    {
        /// <summary>
        /// A issue of test item.
        /// </summary>
        [JsonProperty("issue")]
        public Issue Issue { get; set; }

        /// <summary>
        /// ID of test item to assign the issue.
        /// </summary>
        [JsonProperty("test_item_id")]
        public string TestItemId { get; set; }
    }
}
