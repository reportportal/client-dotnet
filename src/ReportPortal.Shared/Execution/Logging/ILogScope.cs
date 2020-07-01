using ReportPortal.Client.Abstractions.Requests;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Sends log messages to active logging scope.
    /// </summary>
    public interface ILogScope : IDisposable
    {
        /// <summary>
        /// Unique ID of current logging scope.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Parent logging scope.
        /// </summary>
        ILogScope Parent { get; }

        /// <summary>
        /// Root logging scope.
        /// </summary>
        ILogScope Root { get; }

        /// <summary>
        /// Context which current logging scope belong to.
        /// </summary>
        ILogContext Context { get; }

        /// <summary>
        /// Logical login scope name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Time when loging scope began.
        /// </summary>
        DateTime BeginTime { get; }

        /// <summary>
        /// Time when logging scope ended.
        /// </summary>
        DateTime? EndTime { get; }

        /// <summary>
        /// Logging scope status.
        /// </summary>
        LogScopeStatus Status { get; set; }

        /// <summary>
        /// Starts new logging scope beginning from active scope.
        /// </summary>
        /// <param name="name">A name of the scope.</param>
        /// <returns></returns>
        ILogScope BeginScope(string name);

        /// <summary>
        /// Sends log message to current test context.
        /// </summary>
        /// <param name="logRequest">Full model object for message</param>
        void Message(CreateLogItemRequest logRequest);

        /// <summary>
        /// Sends log message with "Info" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        void Info(string message);

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        void Info(string message, string mimeType, byte[] content);

        /// <summary>
        /// Sends log message with "Debug" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        void Debug(string message);

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        void Debug(string message, string mimeType, byte[] content);

        /// <summary>
        /// Sends log message with "Trace" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        void Trace(string message);

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        void Trace(string message, string mimeType, byte[] content);

        /// <summary>
        /// Sends log message with "Error" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        void Error(string message);

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        void Error(string message, string mimeType, byte[] content);

        /// <summary>
        /// Sends log message with "Fatal" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        void Fatal(string message);

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        void Fatal(string message, string mimeType, byte[] content);

        /// <summary>
        /// Sends log message with "Warn" level to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        void Warn(string message);

        /// <summary>
        /// Sends binary content to current test context.
        /// </summary>
        /// <param name="message">Text of the message</param>
        /// <param name="mimeType">Mime type of content</param>
        /// <param name="content">Array of bytes</param>
        void Warn(string message, string mimeType, byte[] content);
    }
}
