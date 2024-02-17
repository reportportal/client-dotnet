using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a list of responses.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public class ResponsesList<T>
    {
        /// <summary>
        /// Gets or sets the list of items.
        /// </summary>
        [JsonPropertyName("responses")]
        public IList<T> Items { get; set; }
    }
}