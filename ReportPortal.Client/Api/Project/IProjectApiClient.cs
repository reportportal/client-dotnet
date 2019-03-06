using ReportPortal.Client.Api.Project.Model;
using ReportPortal.Client.Api.Project.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Api.Project
{
    public interface IProjectApiClient
    {
        /// <summary>
        /// updates the project preference for user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UpdatePreferencesResponse> UpdatePreferencesAsync(UpdatePreferenceRequest model, string userName);

        /// <summary>
        /// gets all user preferences
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Preference> GetAllPreferences(string userName);
    }
}
