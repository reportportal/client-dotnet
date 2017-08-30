using System.Collections.Generic;
using System.Runtime.Serialization;

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
        [DataMember(EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Description of test item.
        /// </summary>
        public string Description { get; set; }
    }
}
