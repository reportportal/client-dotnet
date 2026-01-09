using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Client.Converters;

/// <summary>
/// Provides utility methods for bidirectional conversion between string representations 
/// and <see cref="LogLevel"/> enumeration values. This converter supports standard log level 
/// names and their common variations.
/// </summary>
public static class LogLevelConverter
{
    /// <summary>
    /// Converts a string representation of a log level to its corresponding <see cref="LogLevel"/> enumeration value.
    /// The conversion is case-insensitive and supports standard level names (TRACE, DEBUG, INFO, WARN, WARNING, ERROR, FATAL).
    /// </summary>
    /// <param name="level">The string representation of the log level to parse. Can be null or empty, in which case <see cref="LogLevel.Info"/> is returned.</param>
    /// <returns>The corresponding <see cref="LogLevel"/> enumeration value. Returns <see cref="LogLevel.Info"/> if the input is null or empty.</returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="level"/> string does not match any known log level name.</exception>
    public static LogLevel Parse(string level)
    {
        if (string.IsNullOrEmpty(level)) return LogLevel.Info;
        return level.ToUpperInvariant() switch
        {
            "TRACE" => LogLevel.Trace,
            "DEBUG" => LogLevel.Debug,
            "INFO" => LogLevel.Info,
            "WARN" or "WARNING" => LogLevel.Warning,
            "ERROR" => LogLevel.Error,
            "FATAL" => LogLevel.Fatal,
            _ => throw new ArgumentException($"Unknown log level: {level}", nameof(level))
        };
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
        return level switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Fatal => "FATAL",
            _ => throw new ArgumentException($"Unknown log level: {level}", nameof(level))
        };
    }
}
