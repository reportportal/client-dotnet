using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Api.Launch.Model
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

    public enum AnalyzerMode
    {
        [DataMember(Name = "ALL")]
        All,
        [DataMember(Name = "CURRENT_LAUNCH")]
        CurrentLaunch,
        [DataMember(Name = "LAUNCH_NAME")]
        LaunchName
    }
}
