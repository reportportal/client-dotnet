using ReportPortal.Client.Abstractions.Models;

namespace ReportPortal.Client.Converters;

/// <summary>
///     Provides methods for converting between log level strings and the LogLevel enum.
/// </summary>
public static class LogLevelConverter
{
    /// <summary>
    ///     Converts a string representation of a log level to its LogLevel enum counterpart.
    /// </summary>
    /// <param name="levelText"></param>
    /// <returns></returns>
    public static LogLevel Parse(string levelText)
    {
        if (string.IsNullOrEmpty(levelText)) return LogLevel.Info;
        switch (levelText.ToUpperInvariant())
        {
            case "TRACE": return LogLevel.Trace;
            case "DEBUG": return LogLevel.Debug;
            case "INFO": return LogLevel.Info;
            case "WARN":
            case "WARNING": return LogLevel.Warning;
            case "ERROR": return LogLevel.Error;
            case "FATAL": return LogLevel.Fatal;
            default: return LogLevel.Info;
        }
    }

    /// <summary>
    ///     Converts a LogLevel enum value to its string representation.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static string ToLevelText(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Fatal => "FATAL",
            _ => "INFO"
        };
    }
}