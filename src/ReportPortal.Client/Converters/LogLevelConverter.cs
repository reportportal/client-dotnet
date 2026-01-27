using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Client.Converters;

/// <summary>
/// Provides utility methods for bidirectional conversion between string representations 
/// and <see cref="LogLevel"/> enumeration values. This converter supports standard log level 
/// names and their common variations.
/// </summary>
internal static class LogLevelConverter
{
    /// <summary>
    /// Converts a string representation of a log level to its corresponding <see cref="LogLevel"/> enumeration value.
    /// The conversion is case-insensitive and supports standard level names (TRACE, DEBUG, INFO, WARN, WARNING, ERROR, FATAL).
    /// </summary>
    /// <param name="level">The string representation of the log level to parse. Can be null or empty, in which case <see cref="LogLevel.Info"/> is returned.</param>
    /// <returns>The corresponding <see cref="LogLevel"/> enumeration value. Returns <see cref="LogLevel.Info"/> if the input is null or empty.</returns>
    public static LogLevel Parse(string level)
    {
        switch (level.ToUpperInvariant())
        {
            case "TRACE":
                return LogLevel.Trace;
            case "DEBUG":
                return LogLevel.Debug;
            case "INFO":
                return LogLevel.Info;
            case "WARN":
            case "WARNING":
                return LogLevel.Warning;
            case "ERROR":
                return LogLevel.Error;
            case "FATAL":
                return LogLevel.Fatal;
            default:
                return default;
        }
    }

    /// <summary>
    /// Converts a <see cref="LogLevel"/> enumeration value to its uppercase string representation.
    /// The returned string follows the standard log level naming convention used in ReportPortal.
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/> enumeration value to convert.</param>
    /// <returns>The uppercase string representation of the log level (e.g., "TRACE", "DEBUG", "INFO", "WARN", "ERROR", "FATAL").</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="level"/> value is not a recognized <see cref="LogLevel"/> enumeration member.</exception>
    public static string ToLevelString(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Trace:
                return "TRACE";
            case LogLevel.Debug:
                return "DEBUG";
            case LogLevel.Info:
                return "INFO";
            case LogLevel.Warning:
                return "WARN";
            case LogLevel.Error:
                return "ERROR";
            case LogLevel.Fatal:
                return "FATAL";
            default:
                throw new ArgumentException($"Unknown log level: {level}", nameof(level));
        }
    }
}
