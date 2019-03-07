using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ReportPortal.Client.Api.Launch.Model;
using ReportPortal.Client.Converter;

namespace ReportPortal.Client.Api.Launch.Request
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

        public AnalyzerMode AnalyzerMode
        {
            get => EnumConverter.ConvertTo<AnalyzerMode>(AnalyzerModeString);
            set => AnalyzerModeString = EnumConverter.ConvertFrom(value);
        }

        [DataMember(Name = "analyze_items_mode")]
        public List<string> AnalyzerItemsModeString { get; set; }

        public IReadOnlyList<AnalyzerItemsMode> AnalyzerItemsMode
        {
            get => AnalyzerItemsModeString.Select(EnumConverter.ConvertTo<AnalyzerItemsMode>).ToList();
            set => AnalyzerItemsModeString = value.Select(i => EnumConverter.ConvertFrom(i)).ToList();
        }
    }
}
