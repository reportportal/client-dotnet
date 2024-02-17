using ReportPortal.Client.Abstractions.Requests;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents an interface for reporting logs.
    /// </summary>
    public interface ILogsReporter
    {
        /// <summary>
        /// Gets the processing task for the logs reporter.
        /// </summary>
        Task ProcessingTask { get; }

        /// <summary>
        /// Logs a new log item.
        /// </summary>
        /// <param name="logRequest">The log item to be logged.</param>
        void Log(CreateLogItemRequest logRequest);

        /// <summary>
        /// Synchronizes the logs.
        /// </summary>
        void Sync();
    }
}
