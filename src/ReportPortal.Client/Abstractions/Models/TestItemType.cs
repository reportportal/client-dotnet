using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes types of test items.
    /// </summary>
    public enum TestItemType
    {
        [JsonPropertyName("SUITE")]
        Suite,
        [JsonPropertyName("TEST")]
        Test,
        [JsonPropertyName("STEP")]
        Step,
        [JsonPropertyName("BEFORE_CLASS")]
        BeforeClass,
        [JsonPropertyName("AFTER_CLASS")]
        AfterClass,
        [JsonPropertyName("AFTER_METHOD")]
        AfterMethod,
        [JsonPropertyName("BEFORE_METHOD")]
        BeforeMethod
    }
}
