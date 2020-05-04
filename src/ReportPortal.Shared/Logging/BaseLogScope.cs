using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System;

namespace ReportPortal.Shared.Logging
{
    abstract class BaseLogScope : ILogScope
    {
        protected ILogScopeManager _logScopeManager;

        public BaseLogScope(ILogScopeManager logScopeManager)
        {
            _logScopeManager = logScopeManager;

            BeginTime = DateTime.UtcNow;
        }

        public virtual string Id { get; } = Guid.NewGuid().ToString();

        public virtual ILogScope Parent { get; }

        public virtual string Name { get; }

        public virtual DateTime BeginTime { get; }

        public virtual DateTime? EndTime { get; private set; }

        public virtual LogScopeStatus Status { get; set; } = LogScopeStatus.InProgress;

        public virtual ILogScope BeginScope(string name)
        {
            var logScope = new LogScope(_logScopeManager, this, name);
            _logScopeManager.ActiveScope = logScope;

            return logScope;
        }

        public void Debug(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Debug;
            Message(logRequest);
        }

        public void Debug(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Debug;
            logRequest.Attach = GetAttachFromContent(mimeType, content);
            Message(logRequest);
        }

        public void Error(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Error;
            Message(logRequest);
        }

        public void Error(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Error;
            logRequest.Attach = GetAttachFromContent(mimeType, content);
            Message(logRequest);
        }

        public void Fatal(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Fatal;
            Message(logRequest);
        }

        public void Fatal(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Fatal;
            logRequest.Attach = GetAttachFromContent(mimeType, content);
            Message(logRequest);
        }

        public void Info(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Info;
            Message(logRequest);
        }

        public void Info(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Info;
            logRequest.Attach = GetAttachFromContent(mimeType, content);
            Message(logRequest);
        }

        public void Trace(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Trace;
            Message(logRequest);
        }

        public void Trace(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Trace;
            logRequest.Attach = GetAttachFromContent(mimeType, content);
            Message(logRequest);
        }

        public void Warn(string message)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Warning;
            Message(logRequest);
        }

        public void Warn(string message, string mimeType, byte[] content)
        {
            var logRequest = GetDefaultLogRequest(message);
            logRequest.Level = LogLevel.Warning;
            logRequest.Attach = GetAttachFromContent(mimeType, content);
            Message(logRequest);
        }

        public virtual void Message(CreateLogItemRequest logRequest)
        {
            foreach (var handler in Bridge.LogHandlerExtensions)
            {
                var isHandled = handler.Handle(this, logRequest);

                if (isHandled) break;
            }
        }

        protected CreateLogItemRequest GetDefaultLogRequest(string text)
        {
            var logRequest = new CreateLogItemRequest
            {
                Time = DateTime.UtcNow,
                Text = text
            };

            return logRequest;
        }

        protected Attach GetAttachFromContent(string mimeType, byte[] content)
        {
            return new Attach("attachment_name", mimeType, content);
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
