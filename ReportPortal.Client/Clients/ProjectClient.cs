using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Client.Clients
{
    public class ProjectClient: BaseClient
    {
        public ProjectClient(HttpClient httpCLient, Uri baseUri, string project) : base(httpCLient, baseUri, project)
        {
        }

        /// <summary>
        /// updates the project preference for user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<UpdatePreferencesResponse> UpdatePreferencesAsync(UpdatePreferenceRequest model, string userName)
        {
            var uri = BaseUri.Append($"project/{Project}/preference/{userName}");
            var body = ModelSerializer.Serialize<UpdatePreferenceRequest>(model);
            
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UpdatePreferencesResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        [System.Runtime.Serialization.DataContract]
        public class UpdatePreferencesResponse
        {
            [System.Runtime.Serialization.DataMember(Name = "projectRef")]
            public string ProjectRef { get; set; }
        }

        /// <summary>
        /// gets all user preferences
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<Preference> GetAllPreferences(string userName)
        {
            var uri = BaseUri.Append($"project/{Project}/preference/{userName}");
            
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Preference>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
