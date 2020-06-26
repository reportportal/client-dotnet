using System;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Execution.Log;

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
        /// <summary>
        /// Begins new logged scope aka nested step.
        /// </summary>
        /// <param name="name">Logical operation name.</param>
        /// <returns></returns>
        public static ILogScope BeginScope(string name)
        {
            return Context.Current.Log.BeginScope(name);
        }

        /// <summary>
        /// Returns an instance of rooted scope which you can use to log massages, instead of active scope.
        /// </summary>
        public static ILogScope RootScope => Context.Current.Log.Root;

        /// <summary>
        /// Returns an instance of active scope where your code is running.
        /// This scope is used by all methods by default like <see cref="Info(string)"/> or <see cref="Debug(string, string, byte[])"/>.
        /// </summary>
        public static ILogScope ActiveScope => Context.Current.Log;

        /// <summary>
        /// Sends log message to current test context.
        /// </summary>
        /// <param name="logRequest">Full model object for message</param>
        [Obsolete("This method will removed. If you want to construct CreateLogItemRequest by yourself, please use Log.ActiveScope.Message() method.")]
        public static void Message(CreateLogItemRequest logRequest)
        {
            ActiveScope.Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Info" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Info(string message)
        {
            ActiveScope.Info(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Info(string message, string mimeType, byte[] content)
        {
            ActiveScope.Info(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Debug" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Debug(string message)
        {
            ActiveScope.Debug(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Debug(string message, string mimeType, byte[] content)
        {
            ActiveScope.Debug(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Trace" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Trace(string message)
        {
            ActiveScope.Trace(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Trace(string message, string mimeType, byte[] content)
        {
            ActiveScope.Trace(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Error" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Error(string message)
        {
            ActiveScope.Error(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Error(string message, string mimeType, byte[] content)
        {
            ActiveScope.Error(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Fatal" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Fatal(string message)
        {
            ActiveScope.Fatal(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Fatal(string message, string mimeType, byte[] content)
        {
            ActiveScope.Fatal(message, mimeType, content);
        }

        /// <summary>
        /// Sends log message with "Warn" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Warn(string message)
        {
            ActiveScope.Warn(message);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Warn(string message, string mimeType, byte[] content)
        {
            ActiveScope.Warn(message, mimeType, content);
        }
    }
}
