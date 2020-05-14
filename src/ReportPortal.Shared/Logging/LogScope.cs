using ReportPortal.Shared.Extensibility;
using System;

namespace ReportPortal.Shared.Logging
{
    class LogScope : BaseLogScope
    {
        public LogScope(ILogScopeManager logScopeManager, IExtensionManager extensionManager, ILogScope parent, string name) : base(logScopeManager, extensionManager)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            Parent = parent;
            Name = name;

            foreach (var logHandler in _extensionManager.LogHandlers)
            {
                logHandler.BeginScope(this);
            }
        }

        public override ILogScope Parent { get; }

        public override string Name { get; }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var logHandler in _extensionManager.LogHandlers)
            {
                logHandler.EndScope(this);
            }

            _logScopeManager.ActiveScope = Parent;
        }
    }
}
