using ReportPortal.Client.Abstractions.Models;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    ///     Provides conversion between LogMessageLevel and LogLevel.
    /// </summary>
    public static class LogMessageLevelConverter
    {
        /// <summary>
        ///     Converts a LogMessageLevel to a LogLevel.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
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
    }
}