using ReportPortal.Shared.Reporter.Statistics;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents an interface for a reporter.
    /// </summary>
    public interface IReporter
    {
        /// <summary>
        /// Gets the task that represents the start of the reporter.
        /// </summary>
        Task StartTask { get; }

        /// <summary>
        /// Gets the task that represents the finish of the reporter.
        /// </summary>
        Task FinishTask { get; }

        /// <summary>
        /// Synchronizes the reporter.
        /// </summary>
        void Sync();

        /// <summary>
        /// Gets the statistics counter for the reporter.
        /// </summary>
        ILaunchStatisticsCounter StatisticsCounter { get; }
    }
}
