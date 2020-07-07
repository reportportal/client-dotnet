using System;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Execution.Logging;

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
    [Obsolete("Please use Context.Current.Log class to log messages.")]
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
        public static void Message(CreateLogItemRequest logRequest)
        {
            var message = new LogMessage(logRequest.Text);

            LogMessageLevel logLevel = LogMessageLevel.Info;

            switch (logRequest.Level)
            {
                case LogLevel.Debug:
                    logLevel = LogMessageLevel.Debug;
                    break;
                case LogLevel.Error:
                    logLevel = LogMessageLevel.Error;
                    break;
                case LogLevel.Fatal:
                    logLevel = LogMessageLevel.Fatal;
                    break;
                case LogLevel.Info:
                    logLevel = LogMessageLevel.Info;
                    break;
                case LogLevel.Trace:
                    logLevel = LogMessageLevel.Trace;
                    break;
                case LogLevel.Warning:
                    logLevel = LogMessageLevel.Warning;
                    break;
            }

            message.Level = logLevel;

            message.Time = logRequest.Time;

            if (logRequest.Attach != null)
            {
                message.Attachment = new LogMessageAttachment(logRequest.Attach.MimeType, logRequest.Attach.Data);
            }

            ActiveScope.Message(message);
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