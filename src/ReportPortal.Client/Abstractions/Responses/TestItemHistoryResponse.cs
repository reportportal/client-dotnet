using System.Collections.Generic;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a container for test item history.
    /// </summary>
    public class TestItemHistoryContainer
    {
        /// <summary>
        /// Gets or sets the grouping field.
        /// </summary>
        public string GroupingField { get; set; }

        /// <summary>
        /// Gets or sets the list of test item responses.
        /// </summary>
        public IList<TestItemResponse> Resources { get; set; }
    }
}
