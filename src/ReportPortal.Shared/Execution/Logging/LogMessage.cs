using System;

namespace ReportPortal.Shared.Execution.Logging
{
    public class LogMessage : ILogMessage
    {
        public LogMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Log message cannot be null or empty", nameof(message));

            Message = message;
            Time = DateTime.UtcNow;
            Level = LogMessageLevel.Info;
        }

        public string Message { get; set; }
        public DateTime Time { get; set; }
        public LogMessageLevel Level { get; set; }
        public ILogMessageAttachment Attachment { get; set; }
    }
}
