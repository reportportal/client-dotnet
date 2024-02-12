namespace ReportPortal.Shared.Reporter.Statistics
{
    /// <summary>
    /// Meta-interface to capture statistics about requests execution durations.
    /// </summary>
    public interface ILaunchStatisticsCounter
    {
        /// <summary>
        /// Returns statistics about StartTestItem requests.
        /// </summary>
        IStatisticsCounter StartTestItemStatisticsCounter { get; }

        /// <summary>
        /// Returns statistics about FinishTestItem requests.
        /// </summary>
        IStatisticsCounter FinishTestItemStatisticsCounter { get; }

        /// <summary>
        /// Returns statistics about LogItem requests.
        /// </summary>
        IStatisticsCounter LogItemStatisticsCounter { get; }
    }
}
