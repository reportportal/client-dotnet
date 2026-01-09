using ReportPortal.Client.Abstractions.Models;
using System;
using System.Collections.Generic;

namespace ReportPortal.Client.Converters;

/// <summary>
/// Provides shared, centralized mapping logic for log level string conversions.
/// This internal utility ensures consistency across different log level enum types
/// and eliminates duplication of parsing logic.
/// </summary>
internal static class LogLevelMapping
{
    private static readonly Dictionary<string, LogLevel> StringToLevelMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "TRACE", LogLevel.Trace },
        { "DEBUG", LogLevel.Debug },
        { "INFO", LogLevel.Info },
        { "WARN", LogLevel.Warning },
        { "WARNING", LogLevel.Warning },
        { "ERROR", LogLevel.Error },
        { "FATAL", LogLevel.Fatal }
    };

    /// <summary>
    /// Parses a string representation into a <see cref="LogLevel"/> enum value.
    /// The conversion is case-insensitive and supports standard level names and their variations.
    /// </summary>
    /// <param name="level">The string to parse (case-insensitive). Can be null or empty.</param>
    /// <param name="defaultValue">The default value to return if parsing fails or input is null/empty.</param>
    /// <returns>The parsed <see cref="LogLevel"/> or the specified default value.</returns>
    public static LogLevel ParseToLogLevel(string level, LogLevel defaultValue = LogLevel.Info)
    {
        if (string.IsNullOrEmpty(level))
            return defaultValue;

        return StringToLevelMap.TryGetValue(level, out var result) ? result : defaultValue;
    }
}
