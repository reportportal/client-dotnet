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
            switch (levelString)
            {
                case "TRACE":
                    return LogMessageLevel.Trace;
                case "DEBUG":
                    return LogMessageLevel.Debug;
                case "INFO":
                    return LogMessageLevel.Info;
                case "WARNING":
                case "WARN":
                    return LogMessageLevel.Warning;
                case "ERROR":
                    return LogMessageLevel.Error;
                case "FATAL":
                    return LogMessageLevel.Fatal;
                default:
                    throw new ArgumentException($"Unknown log level: {levelString}", nameof(levelString));
            }
        }

        private static string ToLevelString(LogMessageLevel level)
        {
            switch (level)
            {
                case LogMessageLevel.Trace:
                    return "TRACE";
                case LogMessageLevel.Debug:
                    return "DEBUG";
                case LogMessageLevel.Info:
                    return "INFO";
                case LogMessageLevel.Warning:
                    return "WARN";
                case LogMessageLevel.Error:
                    return "ERROR";
                case LogMessageLevel.Fatal:
                    return "FATAL";
                default:
                    throw new ArgumentException($"Unknown log level: {level}", nameof(level));
            }
        }
    }
}
