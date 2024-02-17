using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents the response for creating a test item.
    /// </summary>
    public class TestItemCreatedResponse
    {
        /// <summary>
        /// Gets or sets the UUID of the created test item.
        /// </summary>
        [JsonPropertyName("id")]
        public string Uuid { get; set; }
    }
}
