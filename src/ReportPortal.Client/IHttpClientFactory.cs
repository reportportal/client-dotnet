using System.Net.Http;

namespace ReportPortal.Client
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
