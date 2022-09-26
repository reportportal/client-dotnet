using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    public class ProjectDefectSubTypesContainer
    {
        [JsonPropertyName("PRODUCT_BUG")]
        public IList<ProjectDefectSubType> ProductBugTypes { get; set; }

        [JsonPropertyName("AUTOMATION_BUG")]
        public IList<ProjectDefectSubType> AutomationBugTypes { get; set; }

        [JsonPropertyName("SYSTEM_ISSUE")]
        public IList<ProjectDefectSubType> SystemIssueTypes { get; set; }

        [JsonPropertyName("TO_INVESTIGATE")]
        public IList<ProjectDefectSubType> ToInvestigateTypes { get; set; }

        [JsonPropertyName("NO_DEFECT")]
        public IList<ProjectDefectSubType> NoDefectTypes { get; set; }
    }
}
