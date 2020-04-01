using System;

namespace ReportPortal.Shared.Logging
{
    class LogScope : BaseLogScope
    {
        public LogScope(ILogScopeManager logScopeManager, ILogScope parent, string name) : base(logScopeManager)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            Parent = parent;
            Name = name;

            foreach (var logHandler in Bridge.LogHandlerExtensions)
            {
                logHandler.BeginScope(this);
            }
        }

        public override ILogScope Parent { get; }

        public override string Name { get; }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var logHandler in Bridge.LogHandlerExtensions)
            {
                logHandler.EndScope(this);
            }

            _logScopeManager.ActiveScope = Parent;
        }
    }
}
