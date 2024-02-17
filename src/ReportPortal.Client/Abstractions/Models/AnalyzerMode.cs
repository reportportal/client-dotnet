using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Represents the mode of the analyzer.
    /// </summary>
    public enum AnalyzerMode
    {
        /// <summary>
        /// Analyzes all launches.
        /// </summary>
        [JsonPropertyName("ALL")]
        All,
        
        /// <summary>
        /// Analyzes the current launch.
        /// </summary>
        [JsonPropertyName("CURRENT_LAUNCH")]
        CurrentLaunch,
        
        /// <summary>
        /// Analyzes launches by launch name.
        /// </summary>
        [JsonPropertyName("LAUNCH_NAME")]
        LaunchName
    }
}
