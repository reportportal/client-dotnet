namespace ReportPortal.Shared.Reporter
{
    /// <summary>
    /// Represents the interface for launch reporter information.
    /// </summary>
    public interface ILaunchReporterInfo : IReporterInfo
    {
        /// <summary>
        /// Gets or sets the URL of the launch reporter.
        /// </summary>
        string Url { get; set; }
    }
}
