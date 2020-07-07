using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    public static class LogMessageExtensions
    {
        public static CreateLogItemRequest ConvertToRequest(ILogMessage logMessage)
        {
            if (logMessage == null) throw new ArgumentNullException("Cannot convert nullable log message object.", nameof(logMessage));

            LogLevel logLevel;

            switch (logMessage.Level)
            {
                case LogMessageLevel.Debug:
                    logLevel = LogLevel.Debug;
                    break;
                case LogMessageLevel.Error:
                    logLevel = LogLevel.Error;
                    break;
                case LogMessageLevel.Fatal:
                    logLevel = LogLevel.Fatal;
                    break;
                case LogMessageLevel.Info:
                    logLevel = LogLevel.Info;
                    break;
                case LogMessageLevel.Trace:
                    logLevel = LogLevel.Trace;
                    break;
                case LogMessageLevel.Warning:
                    logLevel = LogLevel.Warning;
                    break;
                default:
                    throw new Exception(string.Format("Unknown {0} level of log message.", logMessage.Level));
            }

            var logRequest = new CreateLogItemRequest
            {
                Text = logMessage.Message,
                Time = logMessage.Time,
                Level = logLevel
            };

            if (logMessage.Attachment != null)
            {
                logRequest.Attach = new Client.Abstractions.Responses.Attach
                {
                    MimeType = logMessage.Attachment.MimeType,
                    Data = logMessage.Attachment.Data
                };
            }

            return logRequest;
        }
    }
}
