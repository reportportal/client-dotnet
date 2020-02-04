using ReportPortal.Client.Http;
using System;
using System.Net.Http;

namespace ReportPortal.Client.Extentions
{
    static class HttpResponseMessageExtension
    {
        public static void VerifySuccessStatusCode(this HttpResponseMessage httpResponseMessage)
        {
            var requestUri = httpResponseMessage.RequestMessage.RequestUri;
            var body = httpResponseMessage.Content.ReadAsStringAsync().Result;

            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw new ReportPortalException($"Response status code does not indicate success: {httpResponseMessage.StatusCode} ({(int)httpResponseMessage.StatusCode}) {httpResponseMessage.RequestMessage.Method} {requestUri}", new HttpRequestException($"Response message: {body}"));
            }
        }
    }
}
