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
        /// Update attributes for test item.
        /// </summary>
        public List<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Description of test item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// New status for test item.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverterEx<Status>))]
        public Status Status { get; set; }
    }
}
