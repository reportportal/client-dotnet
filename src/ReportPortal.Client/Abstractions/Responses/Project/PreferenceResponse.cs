using System.Collections.Generic;

namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Represents a response containing user preferences for a project.
    /// </summary>
    public class PreferenceResponse
    {
        /// <summary>
        /// Gets or sets the list of filters in a preference.
        /// </summary>
        public IList<UserFilterResponse> Filters { get; set; }

        /// <summary>
        /// Gets or sets the project ID.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public long UserId { get; set; }
    }
}
