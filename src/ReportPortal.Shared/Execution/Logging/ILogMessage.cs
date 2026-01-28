using System;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Represents message for logging.
    /// </summary>
    public interface ILogMessage
    {
        /// <summary>
        /// Textual log event message.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Time representation when log event occurs.
        /// </summary>
        DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the log level of the log message using the strongly-typed <see cref="LogMessageLevel"/> enumeration.
        /// </summary>
        /// <value>A <see cref="LogMessageLevel"/> enumeration value representing the severity level of the log message.</value>
        /// <exception cref="ArgumentException">Thrown when setting a value that corresponds to an invalid or unrecognized log level string, or when getting a value from an invalid <see cref="LevelString"/>.</exception>
        LogMessageLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the log level of the log message as a string value (e.g., "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL").
        /// This property allows for custom or extended log level names beyond the standard <see cref="LogMessageLevel"/> enumeration.
        /// </summary>
        /// <remarks>
        /// This property is supported starting from Report Portal version 25.2, which enables custom log level definitions.
        /// For standard log levels, prefer using the <see cref="Level"/> property for type safety.
        /// </remarks>
        /// <value>A string representing the log level. May contain standard level names or custom level values.</value>
        string LevelString { get; set; }

        /// <summary>
        /// Binary data attached to log event.
        /// Null if log event is without attachment.
        /// </summary>
        ILogMessageAttachment Attachment { get; set; }
    }
}
