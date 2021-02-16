namespace ReportPortal.Shared.Reporter.Statistics
{
    /// <inheritdoc/>
    public class LaunchStatisticsCounter : ILaunchStatisticsCounter
    {
        /// <inheritdoc/>
        public IStatisticsCounter StartTestItemStatisticsCounter { get; } = new StatisticsCounter();

        /// <inheritdoc/>
        public IStatisticsCounter FinishTestItemStatisticsCounter { get; } = new StatisticsCounter();

        /// <inheritdoc/>
        public IStatisticsCounter LogItemStatisticsCounter { get; } = new StatisticsCounter();

        /// <summary>
        /// Returns a string that represents the statistics counter for launch.
        /// </summary>
        /// <returns>A string that represents the statistics counter.</returns>
        public override string ToString()
        {
            return $"ST {StartTestItemStatisticsCounter}, FT {FinishTestItemStatisticsCounter}, L {LogItemStatisticsCounter}";
        }
    }
}
