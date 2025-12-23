using System;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Converters;

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
            if (logMessage == null)
                throw new ArgumentNullException("Cannot convert nullable log message object.", nameof(logMessage));

            var levelString = !string.IsNullOrWhiteSpace(logMessage.LevelText)
                ? logMessage.LevelText.Trim()
                : LogLevelConverter.ToLevelText((LogLevel)logMessage.Level);

            var logRequest = new CreateLogItemRequest
            {
                Text = logMessage.Message,
                Time = logMessage.Time,
                LevelText = levelString
            };

            if (logMessage.Attachment != null)
                logRequest.Attach = new LogItemAttach
                {
                    MimeType = logMessage.Attachment.MimeType,
                    Data = logMessage.Attachment.Data
                };

            return logRequest;
        }
    }
}