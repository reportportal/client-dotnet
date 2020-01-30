using System;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Converters;
using System.Net.Http;
using System.Text;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;

namespace ReportPortal.Client
{
    public class ServiceUserResource : BaseResource, IUserResource
    {
        public ServiceUserResource(HttpClient httpClient, Uri baseUri, string project, string token) : base(httpClient, baseUri, project, token)
        {

        }

        public virtual async Task<User> GetAsync()
        {
            var uri = BaseUri.Append($"user");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
