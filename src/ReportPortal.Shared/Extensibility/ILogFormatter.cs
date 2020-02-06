using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Extensibility
{
    public interface ILogFormatter
    {
        int Order { get; }

        bool FormatLog(CreateLogItemRequest logRequest);
    }
}
