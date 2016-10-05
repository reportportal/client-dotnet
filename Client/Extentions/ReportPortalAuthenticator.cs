using System;
using System.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace ReportPortal.Client.Extentions
{
    public class ReportPortalAuthenticator : IAuthenticator
    {
        private readonly string _authHeader;

        public ReportPortalAuthenticator(string token)
        {
            _authHeader = string.Format("Bearer {0}", token);
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
            {
                request.AddParameter("Authorization", _authHeader, ParameterType.HttpHeader);
            }
        }
    }
}
