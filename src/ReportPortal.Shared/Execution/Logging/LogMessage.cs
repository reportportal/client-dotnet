using System;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <inheritdoc />
    public class LogMessage : ILogMessage
    {
        /// <summary>
        /// Creates new instance of <see href="LogMessage"/> 
        /// </summary>
        /// <param name="message">Textual log event message.</param>
        public LogMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Log message cannot be null or empty", nameof(message));

            Message = message;
            Time = DateTime.UtcNow;
            Level = LogMessageLevel.Info;
        }

        /// <inheritdoc />
        public string Message { get; set; }

        /// <inheritdoc />
        public DateTime Time { get; set; }

        /// <inheritdoc />
        public LogMessageLevel Level { get; set; }

        /// <inheritdoc />
        public ILogMessageAttachment Attachment { get; set; }
    }
}
