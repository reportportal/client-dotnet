using System;
using System.Net.Http;

namespace ReportPortal.Client.Api
{
    public abstract class BaseApiClient : IApiClient
    {
        protected virtual HttpClient HttpClient { get; }
        protected virtual Uri BaseUri { get; }
        protected virtual string Project { get; }

        protected BaseApiClient(HttpClient httpCLient, Uri baseUri, string project)
        {
            HttpClient = httpCLient;
            BaseUri = baseUri;
            Project = project;
        }
    }
}
