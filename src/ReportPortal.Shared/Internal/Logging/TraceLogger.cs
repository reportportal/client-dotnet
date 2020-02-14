using System;
using System.Diagnostics;

namespace ReportPortal.Shared.Internal.Logging
{
    /// <inheritdoc/>
    internal class TraceLogger : ITraceLogger
    {
        private TraceSource _traceSource;

        private int _appDomainId;
        private string _appDomainFriendlyName;

        public TraceLogger(TraceSource traceSource)
        {
            _traceSource = traceSource;

            _appDomainId = AppDomain.CurrentDomain.Id;
            _appDomainFriendlyName = AppDomain.CurrentDomain.FriendlyName;
        }

        public void Info(string message)
        {
            Message(TraceEventType.Information, message);
        }

        public void Verbose(string message)
        {
            Message(TraceEventType.Verbose, message);
        }

        public void Error(string message)
        {
            Message(TraceEventType.Error, message);
        }

        public void Warn(string message)
        {
            Message(TraceEventType.Warning, message);
        }

        private void Message(TraceEventType eventType, string message)
        {
            _traceSource.TraceEvent(eventType, 0, $"{DateTime.Now.ToString("HH:mm:ss.fffffff")} : {_appDomainId}-{_appDomainFriendlyName} : {message}");
        }
    }
}
