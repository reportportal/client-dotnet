using System;
using System.Net.Http;

namespace ReportPortal.Client
{
    public abstract class BaseResource
    {
        public BaseResource(HttpClient httpClient, Uri baseUri, string project, string apiToken)
        {
            HttpClient = httpClient;
            BaseUri = baseUri;
            Project = project;
            ApiToken = apiToken;
        }

        protected HttpClient HttpClient { get; }

        protected Uri BaseUri { get; }

        protected string Project { get; }

        protected string ApiToken { get; }
    }
}
