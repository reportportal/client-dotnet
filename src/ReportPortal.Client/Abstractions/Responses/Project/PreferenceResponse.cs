using System.Collections.Generic;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    public class PreferenceResponse
    {
        /// <summary>
        /// List of filters in a preference.
        /// </summary>
        public IList<UserFilterResponse> Filters { get; set; }

        public long ProjectId { get; set; }

        public long UserId { get; set; }
    }
}
