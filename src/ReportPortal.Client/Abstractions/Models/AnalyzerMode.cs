using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public enum AnalyzerMode
    {
        [JsonPropertyName("ALL")]
        All,
        [JsonPropertyName("CURRENT_LAUNCH")]
        CurrentLaunch,
        [JsonPropertyName("LAUNCH_NAME")]
        LaunchName
    }
}
