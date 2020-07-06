using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Reporter
{
    public interface ILogRequestAmender
    {
        void Amend(CreateLogItemRequest request);
    }
}
