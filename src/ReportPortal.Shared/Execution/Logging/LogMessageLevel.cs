namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Represents the level of a log message.
    /// </summary>
    public enum LogMessageLevel
    {
        /// <summary>
        /// Trace level log message.
        /// </summary>
        Trace,
        
        /// <summary>
        /// Debug level log message.
        /// </summary>
        Debug,
        
        /// <summary>
        /// Information level log message.
        /// </summary>
        Info,
        
        /// <summary>
        /// Warning level log message.
        /// </summary>
        Warning,
        
        /// <summary>
        /// Error level log message.
        /// </summary>
        Error,
        
        /// <summary>
        /// Fatal level log message.
        /// </summary>
        Fatal
    }
}
