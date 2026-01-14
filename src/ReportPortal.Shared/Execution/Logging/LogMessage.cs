using System;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Converters;

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
        }

        /// <inheritdoc />
        public string Message { get; set; }

        /// <inheritdoc />
        public DateTime Time { get; set; }

        /// <inheritdoc />
        public LogMessageLevel Level
        {
            get => ParseLevel(LevelString);
            set => LevelString = ToLevelString(value);
        }

        /// <inheritdoc />
        public string LevelString { get; set; } = "INFO";

        /// <inheritdoc />
        public ILogMessageAttachment Attachment { get; set; }

        private static LogMessageLevel ParseLevel(string levelString)
        {
            var logLevel = LogLevelConverter.Parse(levelString);
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return LogMessageLevel.Trace;
                case LogLevel.Debug:
                    return LogMessageLevel.Debug;
                case LogLevel.Info:
                    return LogMessageLevel.Info;
                case LogLevel.Warning:
                    return LogMessageLevel.Warning;
                case LogLevel.Error:
                    return LogMessageLevel.Error;
                case LogLevel.Fatal:
                    return LogMessageLevel.Fatal;
                default:
                    return LogMessageLevel.Info;
            }
        }

        private static string ToLevelString(LogMessageLevel level)
        {
            LogLevel logLevel;
            switch (level)
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
                    logLevel = LogLevel.Info;
                    break;
            }

            return LogLevelConverter.ToLevelString(logLevel);
        }
    }
}
