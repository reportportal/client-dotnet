using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;

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
        private const string FileName = "attachment_name";

        /// <summary>
        /// Sends log message to current test context.
        /// </summary>
        /// <param name="logRequest">Full model object for message</param>
        public static void Message(CreateLogItemRequest logRequest)
        {
            foreach (var handler in Bridge.LogHandlerExtensions)
            {
                var isHandled = handler.Handle(logRequest);

                if (isHandled) break;
            }
        }

        /// <summary>
        /// Sends log message with "Info" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Info(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Info;
            Message(logRequest);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Info(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Info;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Debug" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Debug(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Debug;
            Message(logRequest);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Debug(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Debug;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Trace" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Trace(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Trace;
            Message(logRequest);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Trace(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Trace;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Error" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Error(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Error;
            Message(logRequest);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Error(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Error;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Fatal" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Fatal(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Fatal;
            Message(logRequest);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Fatal(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Fatal;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        /// <summary>
        /// Sends log message with "Warn" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        public static void Warn(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Warning;
            Message(logRequest);
        }

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        public static void Warn(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Warning;
            logRequest.Attach = GetAttachFromFileInfo(mimeType, content);
            Message(logRequest);
        }

        private static CreateLogItemRequest GetDefaultLogRequest(string text)
        {
            var logRequest = new CreateLogItemRequest
            {
                Time = DateTime.UtcNow,
                Text = text
            };

            return logRequest;
        }

        private static Attach GetAttachFromFileInfo(string mimeType, byte[] content)
        {
            return new Attach(FileName, mimeType, content);
        }
    }
}
