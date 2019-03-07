using System;
using System.Net.Http;
using System.Threading.Tasks;
using ReportPortal.Client.Api.User.Model;
using ReportPortal.Client.Converter;
using ReportPortal.Client.Extention;

namespace ReportPortal.Client.Api.User
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(HttpClient httpClient, Uri baseUri, string project) : base(httpClient, baseUri, project)
        {
        }

        public virtual async Task<UserModel> GetUserAsync()
        {
            var uri = BaseUri.Append("user");
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<UserModel>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
