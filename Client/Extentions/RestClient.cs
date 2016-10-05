using System;
using System.Net;
using ReportPortal.Client.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ReportPortal.Client.Extentions
{
    static class RestClientExtension
    {
        public static IRestResponse ExecuteWithErrorHandling(this RestClient client, RestRequest request)
        {
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                if (response.ErrorException is WebException &&
                    (response.ErrorException.Message.Contains("The underlying connection was closed")
                    || response.ErrorException.Message.Contains("Unable to connect to the remote server")
                    || response.ErrorException.Message.Contains("The operation has timed out")
                    || response.ErrorException.Message.Contains("The remote name could not be resolved")))
                {
                    response = client.Execute(request);
                }

                if (response.ErrorException != null)
                {
                    Console.WriteLine(response.ErrorException);
                    throw response.ErrorException;
                }
            }
            Error errorMessage = null;
            try
            {
                errorMessage = JsonConvert.DeserializeObject<Error>(response.Content);
            }
            catch (JsonReaderException exp)
            {
                throw new Exception(response.Content, exp);
            }
            catch (JsonSerializationException)
            {
            }

            if (errorMessage != null)
            {
                var exp = new ServiceException(errorMessage.Message, errorMessage.Code);
                throw exp;
            }
            return response;
        }
    }
}
