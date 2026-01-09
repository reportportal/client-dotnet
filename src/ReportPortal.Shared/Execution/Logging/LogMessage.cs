using System;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <inheritdoc />
    public class LogMessage : ILogMessage
    {
        private LogMessageLevel _level = LogMessageLevel.Info;
        private string _levelString = "INFO";

        /// <summary>
        /// Creates new instance of <see href="LogMessage"/> 
        /// </summary>
        /// <param name="message">Textual log event message.</param>
        public LogMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Log message cannot be null or empty", nameof(message));

            Message = message;
            Time = DateTime.UtcNow;
        }

        /// <inheritdoc />
        public string Message { get; set; }

        /// <inheritdoc />
        public DateTime Time { get; set; }

        /// <inheritdoc />
        public LogMessageLevel Level
        {
            get => _level;
            set
            {
                _level = value;
                _levelString = LogMessageLevelConverter.ToLevelString(value);
            }
        }

        /// <inheritdoc />
        public string LevelString
        {
            get => _levelString;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _levelString = value;
                    _level = LogMessageLevelConverter.Parse(value);
                }
                else
                {
                    _level = LogMessageLevel.Info;
                    _levelString = "INFO";
                }
            }
        }

        /// <inheritdoc />
        public ILogMessageAttachment Attachment { get; set; }
    }
}
