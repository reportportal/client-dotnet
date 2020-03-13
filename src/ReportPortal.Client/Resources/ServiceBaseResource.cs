using System.Net.Http;

namespace ReportPortal.Client.Resources
{
    abstract class ServiceBaseResource
    {
        public ServiceBaseResource(HttpClient httpClient, string projectName)
        {
            HttpClient = httpClient;
            ProjectName = projectName;
        }

        protected HttpClient HttpClient { get; }

        protected string ProjectName { get; }
    }
}
