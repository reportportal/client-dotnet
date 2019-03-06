using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Api.Project.Model;
using ReportPortal.Client.Api.Project.Request;
using ReportPortal.Client.Converter;
using ReportPortal.Client.Extention;

namespace ReportPortal.Client.Api.Project
{
    public class ProjectApiClient: BaseApiClient, IProjectApiClient
    {
        public ProjectApiClient(HttpClient httpCLient, Uri baseUri, string project) : base(httpCLient, baseUri, project)
        {
        }

        public virtual async Task<UpdatePreferencesResponse> UpdatePreferencesAsync(UpdatePreferenceRequest model, string userName)
        {
            var uri = BaseUri.Append($"project/{Project}/preference/{userName}");
            var body = ModelSerializer.Serialize<UpdatePreferenceRequest>(model);
            
            var response = await HttpClient.PutAsync(uri, new StringContent(body, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UpdatePreferencesResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        public virtual async Task<Preference> GetAllPreferences(string userName)
        {
            var uri = BaseUri.Append($"project/{Project}/preference/{userName}");
            
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<Preference>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
