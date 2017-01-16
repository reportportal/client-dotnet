using Newtonsoft.Json;
using ReportPortal.Client.Extentions;
using ReportPortal.Client.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client
{
    public partial class Service
    {
        public User GetUser()
        {
            var request = new RestRequest("user");
            var response = _restClient.ExecuteWithErrorHandling(request);
            return JsonConvert.DeserializeObject<User>(response.Content);
        }
    }
}
