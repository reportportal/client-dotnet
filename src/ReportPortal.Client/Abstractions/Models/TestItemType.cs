using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Describes types of test items.
    /// </summary>
    public enum TestItemType
    {
        /// <summary>
        /// Represents a test suite.
        /// </summary>
        [JsonPropertyName("SUITE")]
        Suite,

        /// <summary>
        /// Represents a test case.
        /// </summary>
        [JsonPropertyName("TEST")]
        Test,

        /// <summary>
        /// Represents a test step.
        /// </summary>
        [JsonPropertyName("STEP")]
        Step,

        /// <summary>
        /// Represents a before class setup.
        /// </summary>
        [JsonPropertyName("BEFORE_CLASS")]
        BeforeClass,

        /// <summary>
        /// Represents an after class cleanup.
        /// </summary>
        [JsonPropertyName("AFTER_CLASS")]
        AfterClass,

        /// <summary>
        /// Represents an after method cleanup.
        /// </summary>
        [JsonPropertyName("AFTER_METHOD")]
        AfterMethod,

        /// <summary>
        /// Represents a before method setup.
        /// </summary>
        [JsonPropertyName("BEFORE_METHOD")]
        BeforeMethod
    }
}
