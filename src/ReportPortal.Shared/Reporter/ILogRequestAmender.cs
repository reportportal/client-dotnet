using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents an interface for amending log requests.
    /// </summary>
    public interface ILogRequestAmender
    {
        /// <summary>
        /// Amends the specified log request.
        /// </summary>
        /// <param name="request">The log request to be amended.</param>
        void Amend(CreateLogItemRequest request);
    }
}
