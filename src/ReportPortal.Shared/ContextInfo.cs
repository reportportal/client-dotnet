namespace ReportPortal.Shared
{
    public class ContextInfo
    {
        /// <summary>
        /// Current launch ID.
        /// </summary>
        public string LaunchId { get; set; }

        /// <summary>
        /// Current test item ID.
        /// </summary>
        public string TestId { get; set; }

        /// <summary>
        /// Current reporter to send results.
        /// </summary>
        public LaunchReporter LaunchReporter { get; set; }
    }
}