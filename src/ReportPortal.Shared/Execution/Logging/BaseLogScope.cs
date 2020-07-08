using ReportPortal.Shared.Extensibility;
using System;

namespace ReportPortal.Shared.Execution.Logging
{
    abstract class BaseLogScope : ILogScope
    {
        protected IExtensionManager _extensionManager;

        protected CommandsSource _commandsSource;

        public BaseLogScope(ILogContext logContext, IExtensionManager extensionManager, CommandsSource commandsSource)
        {
            Context = logContext;
            _extensionManager = extensionManager;
            _commandsSource = commandsSource;

            BeginTime = DateTime.UtcNow;
        }

        public virtual string Id { get; } = Guid.NewGuid().ToString();

        public virtual ILogScope Parent { get; }

        public virtual ILogScope Root { get; protected set; }

        public virtual ILogContext Context { get; }

        public virtual string Name { get; }

        public virtual DateTime BeginTime { get; }

        public virtual DateTime? EndTime { get; private set; }

        public virtual LogScopeStatus Status { get; set; } = LogScopeStatus.InProgress;

        public virtual ILogScope BeginScope(string name)
        {
            var logScope = new LogScope(Context, _extensionManager, _commandsSource, Root, this, name);

            Context.Log = logScope;

            return logScope;
        }

        public void Debug(string message)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Debug;
            Message(logMessage);
        }

        public void Debug(string message, string mimeType, byte[] content)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Debug;
            logMessage.Attachment = GetAttachFromContent(mimeType, content);
            Message(logMessage);
        }

        public void Error(string message)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Error;
            Message(logMessage);
        }

        public void Error(string message, string mimeType, byte[] content)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Error;
            logMessage.Attachment = GetAttachFromContent(mimeType, content);
            Message(logMessage);
        }

        public void Fatal(string message)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Fatal;
            Message(logMessage);
        }

        public void Fatal(string message, string mimeType, byte[] content)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Fatal;
            logMessage.Attachment = GetAttachFromContent(mimeType, content);
            Message(logMessage);
        }

        public void Info(string message)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Info;
            Message(logMessage);
        }

        public void Info(string message, string mimeType, byte[] content)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Info;
            logMessage.Attachment = GetAttachFromContent(mimeType, content);
            Message(logMessage);
        }

        public void Trace(string message)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Trace;
            Message(logMessage);
        }

        public void Trace(string message, string mimeType, byte[] content)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Trace;
            logMessage.Attachment = GetAttachFromContent(mimeType, content);
            Message(logMessage);
        }

        public void Warn(string message)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Warning;
            Message(logMessage);
        }

        public void Warn(string message, string mimeType, byte[] content)
        {
            var logMessage = GetDefaultLogRequest(message);
            logMessage.Level = LogMessageLevel.Warning;
            logMessage.Attachment = GetAttachFromContent(mimeType, content);
            Message(logMessage);
        }

        public virtual void Message(ILogMessage log)
        {
            CommandsSource.RaiseOnLogMessageCommand(_commandsSource, Context, new Extensibility.Commands.CommandArgs.LogMessageCommandArgs(this, log));
        }

        protected ILogMessage GetDefaultLogRequest(string text)
        {
            var logMessage = new LogMessage(text);

            return logMessage;
        }

        protected ILogMessageAttachment GetAttachFromContent(string mimeType, byte[] content)
        {
            return new LogMessageAttachment(mimeType, content);
        }

        public virtual void Dispose()
        {
            EndTime = DateTime.UtcNow;

            if (Status == LogScopeStatus.InProgress)
            {
                Status = LogScopeStatus.Passed;
            }
        }
    }
}
