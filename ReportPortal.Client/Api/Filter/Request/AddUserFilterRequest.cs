using System.Collections.Generic;
using System.Runtime.Serialization;
using ReportPortal.Client.Api.Filter.Model;

namespace ReportPortal.Client.Api.Filter.Request
{
    /// <summary>
    /// Defines a request for creating of user filters
    /// </summary>
    [DataContract]
    public class AddUserFilterRequest
    {
        /// <summary>
        /// list of filter elements
        /// </summary>
        [DataMember(Name = "elements")]
        public IEnumerable<FilterElement> FilterElements { get; set; }
    }

    
}