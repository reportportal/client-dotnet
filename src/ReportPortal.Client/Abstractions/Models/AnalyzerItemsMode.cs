using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public enum AnalyzerItemsMode
    {
        [DataMember(Name = "TO_INVESTIGATE")]
        ToInvestigate,
        [DataMember(Name = "AUTO_ANALYZED")]
        AutoAnalyzed,
        [DataMember(Name = "MANUALLY_ANALYZED")]
        ManuallyAnalyzed
    }
}
