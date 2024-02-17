namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Represents the status of a logging scope.
    /// </summary>
    public enum LogScopeStatus
    {
        /// <summary>
        /// The logging scope is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The logging scope has passed.
        /// </summary>
        Passed,

        /// <summary>
        /// The logging scope has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The logging scope has been skipped.
        /// </summary>
        Skipped,

        /// <summary>
        /// The logging scope has a warning.
        /// </summary>
        Warn,

        /// <summary>
        /// The logging scope has informational messages.
        /// </summary>
        Info
    }
}
