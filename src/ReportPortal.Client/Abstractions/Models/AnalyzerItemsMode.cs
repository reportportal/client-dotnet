using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    public enum AnalyzerItemsMode
    {
        [JsonPropertyName("TO_INVESTIGATE")]
        ToInvestigate,
        [JsonPropertyName("AUTO_ANALYZED")]
        AutoAnalyzed,
        [JsonPropertyName("MANUALLY_ANALYZED")]
        ManuallyAnalyzed
    }
}
