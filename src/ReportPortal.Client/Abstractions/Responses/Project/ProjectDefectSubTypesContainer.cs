using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Represents a container for project defect subtypes.
    /// </summary>
    public class ProjectDefectSubTypesContainer
    {
        /// <summary>
        /// Gets or sets the list of product bug types.
        /// </summary>
        [JsonPropertyName("PRODUCT_BUG")]
        public IList<ProjectDefectSubType> ProductBugTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of automation bug types.
        /// </summary>
        [JsonPropertyName("AUTOMATION_BUG")]
        public IList<ProjectDefectSubType> AutomationBugTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of system issue types.
        /// </summary>
        [JsonPropertyName("SYSTEM_ISSUE")]
        public IList<ProjectDefectSubType> SystemIssueTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of types to investigate.
        /// </summary>
        [JsonPropertyName("TO_INVESTIGATE")]
        public IList<ProjectDefectSubType> ToInvestigateTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of types with no defect.
        /// </summary>
        [JsonPropertyName("NO_DEFECT")]
        public IList<ProjectDefectSubType> NoDefectTypes { get; set; }
    }
}
