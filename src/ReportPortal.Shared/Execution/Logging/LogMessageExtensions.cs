﻿using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Provides extension methods for converting log messages to log item requests.
    /// </summary>
    public static class LogMessageExtensions
    {
        /// <summary>
        /// Converts a log message to a log item request.
        /// </summary>
        /// <param name="logMessage">The log message to convert.</param>
        /// <returns>A log item request.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the log message is null.</exception>
        public static CreateLogItemRequest ConvertToRequest(this ILogMessage logMessage)
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
                logRequest.Attach = new LogItemAttach
                {
                    MimeType = logMessage.Attachment.MimeType,
                    Data = logMessage.Attachment.Data
                };
            }

            return logRequest;
        }
    }
}
