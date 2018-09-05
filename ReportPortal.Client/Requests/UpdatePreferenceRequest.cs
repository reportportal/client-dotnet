using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Requests
{
    [DataContract]
    public class UpdatePreferenceRequest
    {
        /// <summary>
        /// list of filter ids to update with
        /// </summary>
        [DataMember(Name = "filters")]
        public IEnumerable<string> filderiDs { get; set; }

        /// <summary>
        /// flag active. seems like nbot used in API
        /// </summary>
        [DataMember(Name = "active")]
        public string Active { get; set; }
    }
}
