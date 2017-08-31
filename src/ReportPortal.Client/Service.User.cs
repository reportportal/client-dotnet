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
            var uri = new Uri($"{BaseUri}/user");

            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            return ModelSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
        }
    }
}
