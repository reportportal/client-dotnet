using System.Collections.Generic;

namespace ReportPortal.Client.Abstractions.Responses
{
    public class TestItemHistoryContainer
    {
        public string GroupingField { get; set; }

        public IList<TestItemResponse> Resources { get; set; }
    }
}
