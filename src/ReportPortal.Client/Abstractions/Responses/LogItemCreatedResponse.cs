using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents the response for creating log items.
    /// </summary>
    public class LogItemsCreatedResponse
    {
        /// <summary>
        /// Gets or sets the list of log items created.
        /// </summary>
        [JsonPropertyName("responses")]
        public IList<LogItemCreatedResponse> LogItems { get; set; }
    }

    /// <summary>
    /// Represents the response for creating a log item.
    /// </summary>
    public class LogItemCreatedResponse
    {
        /// <summary>
        /// Gets or sets the UUID of the created log item.
        /// </summary>
        [JsonPropertyName("id")]
        public string Uuid { get; set; }
    }
}
