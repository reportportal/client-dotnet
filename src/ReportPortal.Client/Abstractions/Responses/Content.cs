using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Responses
{
    /// <summary>
    /// Represents a generic content response.
    /// </summary>
    /// <typeparam name="T">The type of the content items.</typeparam>
    public class Content<T>
    {
        /// <summary>
        /// Gets or sets the list of content items.
        /// </summary>
        [JsonPropertyName("content")]
        public IList<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the page information.
        /// </summary>
        public Page Page { get; set; }
    }

    /// <summary>
    /// Represents the page information.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the total number of elements.
        /// </summary>
        public int TotalElements { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Number { get; set; }
    }
}
