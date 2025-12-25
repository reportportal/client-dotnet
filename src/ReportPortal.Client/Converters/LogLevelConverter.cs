using ReportPortal.Client.Abstractions.Models;
using System;

namespace ReportPortal.Client.Converters;

/// <summary>
///     Provides methods for converting between log level strings and the LogLevel enum.
/// </summary>
public static class LogLevelConverter
{
    /// <summary>
    ///     Converts a string representation of a log level to its LogLevel enum counterpart.
    /// </summary>
    /// <param name="levelString"></param>
    /// <returns></returns>
    public static LogLevel Parse(string levelString)
    {
        if (string.IsNullOrEmpty(levelString)) return LogLevel.Info;
        return levelString.ToUpperInvariant() switch
        {
            "TRACE" => LogLevel.Trace,
            "DEBUG" => LogLevel.Debug,
            "INFO" => LogLevel.Info,
            "WARN" or "WARNING" => LogLevel.Warning,
            "ERROR" => LogLevel.Error,
            "FATAL" => LogLevel.Fatal,
            _ => throw new ArgumentException($"Unknown log level: {levelString}", nameof(levelString))
        };
    }

    /// <summary>
    ///     Converts a LogLevel enum value to its string representation.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
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