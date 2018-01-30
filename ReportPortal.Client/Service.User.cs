using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Client
{
    public partial class Service
    {
        public async Task<User> GetUserAsync()
        {
            var uri = BaseUri.Append($"user");
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            response.VerifySuccessStatusCode();
            return ModelSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
