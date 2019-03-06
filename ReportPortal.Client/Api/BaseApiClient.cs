using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ReportPortal.Client.Api
{
    public abstract class BaseApiClient : IApiClient
    {
        protected virtual HttpClient HttpClient { get; private set; }
        protected virtual Uri BaseUri { get; private set; }
        protected virtual string Project { get; private set; }

        public BaseApiClient(HttpClient httpCLient, Uri baseUri, string project)
        {
            HttpClient = httpCLient;
            BaseUri = baseUri;
            Project = project;
        }
    }
}
