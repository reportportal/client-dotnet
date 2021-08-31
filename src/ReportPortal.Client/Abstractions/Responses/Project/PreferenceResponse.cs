﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    [DataContract]
    public class PreferenceResponse
    {
        /// <summary>
        /// List of filters in a preference.
        /// </summary>
        [DataMember(Name = "filters")]
        public IList<UserFilterResponse> Filters { get; set; }

        [DataMember(Name = "projectId")]
        public long ProjectId { get; set; }

        [DataMember(Name = "userId")]
        public long UserId { get; set; }
    }
}
