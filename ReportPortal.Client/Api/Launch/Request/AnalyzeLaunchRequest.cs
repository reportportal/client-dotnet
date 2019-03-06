using ReportPortal.Client.Converter;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using ReportPortal.Client.Api.Launch.Model;

namespace ReportPortal.Client.Api.Launch.Requests
{
    /// <summary>
    /// Defines a request to analyze launch.
    /// </summary>
    [DataContract]
    public class AnalyzeLaunchRequest
    {
        [DataMember(Name = "launch_id")]
        public string LaunchId { get; set; }

        [DataMember(Name = "analyzer_mode")]
        public string AnalyzerModeString { get; set; }

        public AnalyzerMode AnalyzerMode { get { return EnumConverter.ConvertTo<AnalyzerMode>(AnalyzerModeString); } set { AnalyzerModeString = EnumConverter.ConvertFrom(value); } }

        [DataMember(Name = "analyze_items_mode")]
        public List<string> AnalyzerItemsModeString { get; set; }

        public IReadOnlyList<AnalyzerItemsMode> AnalyzerItemsMode { get { return AnalyzerItemsModeString.Select(i => EnumConverter.ConvertTo<AnalyzerItemsMode>(i)).ToList(); } set { AnalyzerItemsModeString = value.Select(i => EnumConverter.ConvertFrom(i)).ToList(); } }
    }
}
