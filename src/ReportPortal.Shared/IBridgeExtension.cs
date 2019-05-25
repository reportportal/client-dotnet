using ReportPortal.Client.Requests;

namespace ReportPortal.Shared
{
    public interface IBridgeExtension
    {
        int Order { get; }

        bool Handled { get; set; }

        void FormatLog(ref AddLogItemRequest logRequest);
    }
}
