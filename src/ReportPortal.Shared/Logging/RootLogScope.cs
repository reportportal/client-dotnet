using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Logging
{
    class RootLogScope : ILogScope
    {
        private ILogScopeManager _logScopeManager;

        public RootLogScope(ILogScopeManager logScopeManager)
        {
            _logScopeManager = logScopeManager;
        }

        public void Dispose()
        {

        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public ILogScope BeginNewScope(string name)
        {
            var logScope = new LogScope(_logScopeManager, this, name);
            _logScopeManager.ActiveScope = logScope;

            return logScope;
        }
    }
}
