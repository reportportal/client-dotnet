using ReportPortal.Shared.Extensibility.ReportEvents;

namespace ReportPortal.Shared.Extensibility
{
    /// <summary>
    /// Represents an interface for observing report events.
    /// </summary>
    public interface IReportEventsObserver
    {
        /// <summary>
        /// Initializes the report events observer with the specified report events source.
        /// </summary>
        /// <param name="reportEventsSource">The report events source to initialize with.</param>
        void Initialize(IReportEventsSource reportEventsSource);
    }
}
