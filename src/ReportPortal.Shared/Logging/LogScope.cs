using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Logging
{
    class LogScope : ILogScope
    {
        private ILogScopeManager _logScopeManager;

        private ILogScope _parent;

        public LogScope(ILogScopeManager logScopeManager, ILogScope parent, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Log scope name cannot be null of empty.", nameof(name));
            }

            _logScopeManager = logScopeManager;
            _parent = parent;
        }

        public void Dispose()
        {
            _logScopeManager.ActiveScope = _parent;
        }

        public void Info(string message)
        {

        }

        public ILogScope BeginNewScope(string name)
        {
            var childScope = new LogScope(_logScopeManager, this, name);
            _logScopeManager.ActiveScope = childScope;

            return childScope;
        }
    }
}
