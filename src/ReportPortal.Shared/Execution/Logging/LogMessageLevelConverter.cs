using ReportPortal.Client.Abstractions.Models;
using System;
using ReportPortal.Client.Converters;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Provides utility methods for bidirectional conversion between string representations 
    /// and <see cref="LogMessageLevel"/> enumeration values, as well as conversion to <see cref="LogLevel"/>.
    /// </summary>
    public static class LogMessageLevelConverter
    {
        /// <summary>
        /// Converts a LogMessageLevel to a LogLevel.
        /// </summary>
        /// <param name="level">The log message level to convert.</param>
        /// <returns>The corresponding LogLevel enum value.</returns>
        public static LogLevel ToLogLevel(LogMessageLevel level)
        {
            switch (level)
            {
                case LogMessageLevel.Debug: return LogLevel.Debug;
                case LogMessageLevel.Error: return LogLevel.Error;
                case LogMessageLevel.Fatal: return LogLevel.Fatal;
                case LogMessageLevel.Info: return LogLevel.Info;
                case LogMessageLevel.Trace: return LogLevel.Trace;
                case LogMessageLevel.Warning: return LogLevel.Warning;
                default: return LogLevel.Info;
            }
        }

        /// <summary>
        /// Converts a <see cref="LogLevel"/> to a <see cref="LogMessageLevel"/>.
        /// </summary>
        /// <param name="level">The log level to convert.</param>
        /// <returns>The corresponding LogMessageLevel enum value.</returns>
        private static LogMessageLevel ToLogMessageLevel(LogLevel level)
        {
            switch (level)
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

        /// <summary>
        /// Converts a string representation of a log level to its corresponding <see cref="LogMessageLevel"/> enumeration value.
        /// The conversion is case-insensitive and supports standard level names (TRACE, DEBUG, INFO, WARN, WARNING, ERROR, FATAL).
        /// </summary>
        /// <param name="level">The string representation of the log level to parse. Can be null or empty.</param>
        /// <returns>
        /// The corresponding <see cref="LogMessageLevel"/> enumeration value. 
        /// Returns <see cref="LogMessageLevel.Info"/> if the input is null, empty, or does not match any known log level name.
        /// </returns>
        public static LogMessageLevel Parse(string level)
        {
            var logLevel = LogLevelConverter.Parse(level);
            return ToLogMessageLevel(logLevel);
        }

        /// <summary>
        /// Converts a <see cref="LogMessageLevel"/> enumeration value to its uppercase string representation.
        /// The returned string follows the standard log level naming convention used in ReportPortal.
        /// </summary>
        /// <param name="level">The <see cref="LogMessageLevel"/> enumeration value to convert.</param>
        /// <returns>The uppercase string representation of the log level (e.g., "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL").</returns>
        public static string ToLevelString(LogMessageLevel level)
        {
            return LogLevelConverter.ToLevelString(ToLogLevel(level));
        }
    }
}