using ReportPortal.Client.Abstractions.Requests;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public interface ILogsReporter
    {
        Task ProcessingTask { get; }

        void Log(CreateLogItemRequest logRequest);
    }
}