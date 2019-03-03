using System;
using System.Net.Http;
using System.Threading.Tasks;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;

namespace ReportPortal.Client.Clients
{
    public class UserClient: BaseClient

    {
    public virtual async Task<User> GetUserAsync()
    {
        var uri = BaseUri.Append($"user");
        var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
        response.VerifySuccessStatusCode();
        return ModelSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }

        public UserClient(HttpClient httpCLient, Uri baseUri, string project) : base(httpCLient, baseUri, project)
        {
        }
    }
}
