using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class Preference
    {
        [DataMember(Name = "userRef")]
        public string UserRef { get; set; }

        [DataMember(Name = "projectRef")]
        public string ProjectRef { get; set; }

        [DataMember(Name = "active")]
        public string Active { get; set; }

        /// <summary>
        /// list of filters in a preference
        /// </summary>
        [DataMember(Name = "filters")]
        public List<string> FilterIds { get; set; }
    }
}
