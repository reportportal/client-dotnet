using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    [DataContract]
    public class ProjectDefectSubTypesContainer
    {
        [DataMember(Name = "PRODUCT_BUG")]
        public IList<ProjectDefectSubType> ProductBugTypes { get; set; }

        [DataMember(Name = "AUTOMATION_BUG")]
        public IList<ProjectDefectSubType> AutomationBugTypes { get; set; }

        [DataMember(Name = "SYSTEM_ISSUE")]
        public IList<ProjectDefectSubType> SystemIssueTypes { get; set; }

        [DataMember(Name = "TO_INVESTIGATE")]
        public IList<ProjectDefectSubType> ToInvestigateTypes { get; set; }

        [DataMember(Name = "NO_DEFECT")]
        public IList<ProjectDefectSubType> NoDefectTypes { get; set; }
    }
}
