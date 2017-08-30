using ReportPortal.Client.Converters;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;
using RestSharp;

namespace ReportPortal.Client
{
    public partial class Service
    {
        public User GetUser()
        {
            var request = new RestRequest("user");
            var response = _restClient.ExecuteWithErrorHandling(request);
            return ModelSerializer.Deserialize<User>(response.Content);
        }
    }
}
