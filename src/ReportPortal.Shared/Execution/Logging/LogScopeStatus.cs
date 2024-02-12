namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Status of logging scope.
    /// </summary>
    public enum LogScopeStatus
    {
        InProgress,
        Passed,
        Failed,
        Skipped,
        Warn,
        Info
    }
}
