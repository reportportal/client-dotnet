using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
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

        public string AnalyzerTypeName { get; set; }

        [JsonPropertyName("analyzeItemsMode")]
        [JsonConverter(typeof(JsonStringEnumConverterEx<AnalyzerItemsMode>))]
        public List<AnalyzerItemsMode> AnalyzerItemsMode { get; set; }
    }
}
