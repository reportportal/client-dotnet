using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Logging;

namespace ReportPortal.Shared
{
    /// <summary>
    /// Attach artifacts to current test context like textual messages or binary files.
    /// </summary>
    /// <example>
    /// Usage:
    /// - sends simple message
    /// <code>Log.Info("simple message")</code>
    /// - send image to report
    /// <code>Log.Debug("my screenshot", "image/png", File.ReadAllBytes(file_path))</code>
    /// </example>
    public static class Log
    {
        private static readonly ILogScopeManager _logScopeManager = LogScopeManager.Instance;

        /// <summary>
        /// Begins new logged scope aka nested step.
        /// </summary>
        /// <param name="name">Logical operation name.</param>
        /// <returns></returns>
        public static ILogScope BeginNewScope(string name)
        {
            return _logScopeManager.ActiveScope.BeginNewScope(name);
        }

        /// <summary>
        /// Returns an instance of rooted scope which you can use to log massages, instead of active scope.
        /// </summary>
        public static ILogScope RootScope => _logScopeManager.RootScope;

        /// <summary>
        /// Returns an instance of active scope where your code is running.
        /// This scope is used by all methods by default like <see cref="Info(string)"/> or <see cref="Debug(string, string, byte[])"/>.
        /// </summary>
        public static ILogScope ActiveScope => _logScopeManager.ActiveScope;

        /// <summary>
        /// Sends log message to current test context.
        /// </summary>
        /// <param name="logRequest">Full model object for message</param>
        public static void Message(CreateLogItemRequest logRequest)
        {
            _logScopeManager.ActiveScope.Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Info" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Info(string message)
        {
            _logScopeManager.ActiveScope.Info(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Info(string message, string mimeType, byte[] content)
        {
            _logScopeManager.ActiveScope.Info(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Debug" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Debug(string message)
        {
            _logScopeManager.ActiveScope.Debug(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Debug(string message, string mimeType, byte[] content)
        {
            _logScopeManager.ActiveScope.Debug(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Trace" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Trace(string message)
        {
            _logScopeManager.ActiveScope.Trace(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Trace(string message, string mimeType, byte[] content)
        {
            _logScopeManager.ActiveScope.Trace(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Error" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Error(string message)
        {
            _logScopeManager.ActiveScope.Error(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Error(string message, string mimeType, byte[] content)
        {
            _logScopeManager.ActiveScope.Error(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Fatal" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Fatal(string message)
        {
            _logScopeManager.ActiveScope.Fatal(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Fatal(string message, string mimeType, byte[] content)
        {
            _logScopeManager.ActiveScope.Fatal(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Warn" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Warn(string message)
        {
            _logScopeManager.ActiveScope.Warn(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Warn(string message, string mimeType, byte[] content)
        {
            _logScopeManager.ActiveScope.Warn(message, mimeType, content);
        }
    }
}
