using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Api.Project.Request
{
    [DataContract]
    public class UpdatePreferenceRequest
    {
        /// <summary>
        /// list of filter ids to update with
        /// </summary>
        [DataMember(Name = "filters")]
        public IEnumerable<string>  FilderIds { get; set; }

        /// <summary>
        /// flag active. seems like nbot used in API
        /// </summary>
        [DataMember(Name = "active")]
        public string Active { get; set; }
    }

    [DataContract]
    public class UpdatePreferencesResponse
    {
        [DataMember(Name = "projectRef")]
        public string ProjectRef { get; set; }
    }
}
