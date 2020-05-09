using System.Net.Http;

namespace ReportPortal.Client
{
    /// <summary>
    /// A factory abstraction that can create instances of <see cref="HttpClient" /> when required.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates new instance of <see cref="HttpClient" />.
        /// </summary>
        HttpClient Create();
    }
}
