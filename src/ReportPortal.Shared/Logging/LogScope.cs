using System;

namespace ReportPortal.Shared.Logging
{
    class LogScope : BaseLogScope
    {
        private ILogScope _parent;

        public LogScope(ILogScopeManager logScopeManager, ILogScope parent, string name) : base(logScopeManager)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            _parent = parent;

            foreach (var logHandler in Bridge.LogHandlerExtensions)
            {
                logHandler.BeginScope(this);
            }
        }

        public override void Dispose()
        {
            foreach (var logHandler in Bridge.LogHandlerExtensions)
            {
                logHandler.EndScope(this);
            }

            _logScopeManager.ActiveScope = _parent;
        }
    }
}
