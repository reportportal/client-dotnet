using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Represents an attribute of an item.
    /// </summary>
    public class ItemAttribute
    {
        /// <summary>
        /// Gets or sets the key of the attribute.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the attribute.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is a system attribute.
        /// </summary>
        [JsonPropertyName("system")]
        public bool IsSystem { get; set; }
    }
}
