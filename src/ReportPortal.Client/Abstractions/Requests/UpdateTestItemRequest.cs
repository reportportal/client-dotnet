using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    public class UpdateTestItemRequest
    {
        /// <summary>
        /// Gets or sets the attributes for the test item.
        /// </summary>
        public List<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the description of the test item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the new status for the test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; }
    }
}
