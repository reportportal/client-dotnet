using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to analyze a launch.
    /// </summary>
    public class AnalyzeLaunchRequest
    {
        /// <summary>
        /// Gets or sets the ID of the launch to be analyzed.
        /// </summary>
        public long LaunchId { get; set; }

        /// <summary>
        /// Gets or sets the mode of the analyzer.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<AnalyzerMode>))]
        public AnalyzerMode AnalyzerMode { get; set; }

        /// <summary>
        /// Gets or sets the name of the analyzer type.
        /// </summary>
        public string AnalyzerTypeName { get; set; }

        /// <summary>
        /// Gets or sets the mode of the analyzer items.
        /// </summary>
        [JsonPropertyName("analyzeItemsMode")]
        public List<string> AnalyzerItemsMode { get; set; }
    }
}
