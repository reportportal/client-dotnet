using System;
using System.Net.Http;

namespace ReportPortal.Client
{
    public abstract class BaseResource
    {
        public BaseResource(HttpClient httpClient, Uri baseUri, string project, string token)
        {
            HttpClient = httpClient;
            BaseUri = baseUri;
            Project = project;
            Password = token;
        }

        protected HttpClient HttpClient { get; }

        protected Uri BaseUri { get; }

        protected string Project { get; }

        protected string Password { get; }
    }
}
