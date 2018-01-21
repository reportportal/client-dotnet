using System;
using System.Net.Http;

namespace ReportPortal.Client.Extentions
{
    static class HttpResponseMessageExtension
    {
        public static void VerifySuccessStatusCode(this HttpResponseMessage httpResponseMessage)
        {
            var body = httpResponseMessage.Content.ReadAsStringAsync().Result;

            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch(Exception exp)
            {
                throw new HttpRequestException(body, exp);
            }
        }
    }
}
