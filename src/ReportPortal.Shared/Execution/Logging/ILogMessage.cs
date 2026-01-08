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
        /// Log level of log item using the strongly-typed <see cref="LogMessageLevel"/> enumeration.
        /// </summary>
        LogMessageLevel Level { get; set; }

        /// <summary>
        /// Textual custom log level of log event. Supported from Report Portal Version 25.2.
        /// </summary>
        string LevelString { get; set; }

        /// <summary>
        /// Binary data attached to log event.
        /// Null if log event is without attachment.
        /// </summary>
        ILogMessageAttachment Attachment { get; set; }
    }
}
