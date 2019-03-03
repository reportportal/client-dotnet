using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ReportPortal.Client.Clients
{
    public class BaseClient
    {
        protected HttpClient HttpClient { get; private set; }
        protected Uri BaseUri { get; private set; }
        protected string Project { get; private set; }

        public BaseClient(HttpClient httpCLient, Uri baseUri, string project)
        {
            HttpClient = httpCLient;
            BaseUri = baseUri;
            Project = project;
        }
    }
}
