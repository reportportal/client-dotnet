using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to analyze launch.
    /// </summary>
    public class AnalyzeLaunchRequest
    {
        public long LaunchId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverterEx<AnalyzerMode>))]
        public AnalyzerMode AnalyzerMode { get; set; }

        [DataMember(Name = "analyzeItemsMode")]
        public List<string> AnalyzerItemsModeString { get; set; }

        public string AnalyzerTypeName { get; set; }

        public List<AnalyzerItemsMode> AnalyzerItemsMode { get { return AnalyzerItemsModeString.Select(i => EnumConverter.ConvertTo<AnalyzerItemsMode>(i)).ToList(); } set { AnalyzerItemsModeString = value.Select(i => EnumConverter.ConvertFrom(i)).ToList(); } }
    }
}
