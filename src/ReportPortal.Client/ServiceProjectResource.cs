using System;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using System.Net.Http;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Resources;

namespace ReportPortal.Client
{
    public class ServiceProjectResource : BaseResource, IProjectResource
    {
        public ServiceProjectResource(HttpClient httpClient, Uri baseUri, string project) : base(httpClient, baseUri, project)
        {

        }

        /// <summary>
        /// updates the project preference for user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<MessageResponse> UpdatePreferencesAsync(string projectName, string userName, long filterId)
        {
            var uri = BaseUri.Append($"project/{projectName}/preference/{userName}/{filterId}");

            var response = await HttpClient.PutAsync(uri, new StringContent(string.Empty)).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<MessageResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// gets all user preferences
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<PreferenceResponse> GetAllPreferences(string projectName, string userName)
        {
            var uri = BaseUri.Append($"project/{projectName}/preference/{userName}");

            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<PreferenceResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
