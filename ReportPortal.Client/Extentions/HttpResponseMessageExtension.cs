using System;
using System.Collections.Generic;
using System.Net;
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

        public static bool IsServerError(this HttpResponseMessage httpResponseMessage)
        {
            var serverResponseCodes = new List<HttpStatusCode> { HttpStatusCode.InternalServerError, HttpStatusCode.NotImplemented, HttpStatusCode.BadGateway, HttpStatusCode.ServiceUnavailable, HttpStatusCode.GatewayTimeout, HttpStatusCode.HttpVersionNotSupported };
            return serverResponseCodes.Contains(httpResponseMessage.StatusCode);
        }
    }
}
