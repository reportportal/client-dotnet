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
            catch(Exception exp)
            {
                throw new HttpRequestException($"Unexpected response status code. Request URI: {requestUri}{Environment.NewLine}Response Body: {body}", exp);
            }
        }
    }
}
