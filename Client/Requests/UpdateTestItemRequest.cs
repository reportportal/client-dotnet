using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    public class UpdateTestItemRequest
    {
        /// <summary>
        /// Update tags for test item.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of test item.
        /// </summary>
        public string Description { get; set; }
    }
}
