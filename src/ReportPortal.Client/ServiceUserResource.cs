using System;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using System.Net.Http;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Resources;

namespace ReportPortal.Client
{
    public class ServiceUserResource : BaseResource, IUserResource
    {
        public ServiceUserResource(HttpClient httpClient, Uri baseUri, string project) : base(httpClient, baseUri, project)
        {

        }

        public virtual async Task<UserResponse> GetAsync()
        {
            var uri = BaseUri.Append($"user");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
