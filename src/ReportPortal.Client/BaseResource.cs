using System;
using System.Net.Http;

namespace ReportPortal.Client
{
    public abstract class BaseResource
    {
        public BaseResource(HttpClient httpClient, Uri baseUri, string projectName, string token)
        {
            HttpClient = httpClient;
            BaseUri = baseUri;
            ProjectName = projectName;
            Token = token;
        }

        protected HttpClient HttpClient { get; }

        protected Uri BaseUri { get; }

        protected string ProjectName { get; }

        protected string Token { get; }
    }
}
