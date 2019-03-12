using System;
using System.Net.Http;

namespace ReportPortal.Client.Extention
{
    public static class HttpResponseMessageExtension
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
                throw new HttpRequestException($"Unexpected response status code. {httpResponseMessage.RequestMessage.Method} {requestUri}{Environment.NewLine}Response Body: {body}", exp);
            }
        }
    }
}
