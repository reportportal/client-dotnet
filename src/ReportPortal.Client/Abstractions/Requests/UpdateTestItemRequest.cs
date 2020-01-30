using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    [DataContract]
    public class UpdateTestItemRequest
    {
        /// <summary>
        /// Update tags for test item.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of test item.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
