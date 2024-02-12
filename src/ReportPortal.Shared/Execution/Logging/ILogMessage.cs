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
        /// Level of log event.
        /// </summary>
        LogMessageLevel Level { get; set; }

        /// <summary>
        /// Binary data attached to log event.
        /// Null if log event is without attachment.
        /// </summary>
        ILogMessageAttachment Attachment { get; set; }
    }
}
