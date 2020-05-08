using System;
using System.Net.Http;

namespace ReportPortal.Client.Extentions
{
    static class HttpResponseMessageExtension
    {
        public static void VerifySuccessStatusCode(this HttpResponseMessage httpResponseMessage)
        {
            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                var requestUri = httpResponseMessage.RequestMessage.RequestUri;
                var body = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new ReportPortalException($"Response status code does not indicate success: {httpResponseMessage.StatusCode} ({(int)httpResponseMessage.StatusCode}) {httpResponseMessage.RequestMessage.Method} {requestUri}", new HttpRequestException($"Response message: {body}"));
            }
        }
    }
}
