using System;

namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents the information about a reporter.
    /// </summary>
    public interface IReporterInfo
    {
        /// <summary>
        /// Gets the unique identifier of the reporter.
        /// </summary>
        string Uuid { get; }

        /// <summary>
        /// Gets the name of the reporter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the start time of the reporter.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets the finish time of the reporter.
        /// </summary>
        DateTime? FinishTime { get; }
    }
}
