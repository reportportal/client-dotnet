namespace ReportPortal.Shared.Internal.Logging
{
    /// <summary>
    /// Interface to write internal log messages to file
    /// </summary>
    public interface ITraceLogger
    {
        /// <summary>
        /// Writes log message with "Info" level.
        /// </summary>
        /// <param name="message">Your internal log message</param>
        void Info(string message);

        /// <summary>
        /// Writes log message with "Verbose" level.
        /// </summary>
        /// <param name="message">Your internal log message</param>
        void Verbose(string message);

        /// <summary>
        /// Writes log message with "Warning" level.
        /// </summary>
        /// <param name="message">Your internal log message</param>
        void Warn(string message);

        /// <summary>
        /// Writes log message with "Error" level.
        /// </summary>
        /// <param name="message">Your internal log message</param>
        void Error(string message);
    }
}
