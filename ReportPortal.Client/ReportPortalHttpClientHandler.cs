using ReportPortal.Client.Extentions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client
{
    public class ReportPortalHttpClientHandler : HttpClientHandler
    {
        public int MaxRetries { get; private set; }

        public ReportPortalHttpClientHandler(int maxRetries)
        {
            MaxRetries = maxRetries;

            if (SupportsAutomaticDecompression)
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }
        }

        public ReportPortalHttpClientHandler(int maxRetries, IWebProxy proxy)
            : this(maxRetries)
        {
            Proxy = proxy;
        }

        private List<HttpStatusCode> ResponseStatusCodesForRetrying => new List<HttpStatusCode> { HttpStatusCode.InternalServerError, HttpStatusCode.NotImplemented, HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout, HttpStatusCode.MethodNotAllowed };

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    response = await base.SendAsync(request, cancellationToken);

                    if (!ResponseStatusCodesForRetrying.Contains(response.StatusCode))
                    {
                        return response;
                    }
                    else
                    {
                        response.VerifySuccessStatusCode();
                    }
                }

                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < MaxRetries - 1)
                    {
                        await Task.Delay((int)Math.Pow(2, i + MaxRetries) * 1000);
                    }

                    else
                    {
                        throw;
                    }
                }
            }

            return response;
        }
    }
}
