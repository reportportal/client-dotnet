using ReportPortal.Client.Requests;

namespace ReportPortal.Shared.Extensibility
{
    public interface ILogFormatter
    {
        int Order { get; }

        bool FormatLog(ref AddLogItemRequest logRequest);
    }
}
