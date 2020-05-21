using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Threading;

namespace ReportPortal.Shared.Logging
{
    /// <summary>
    /// Context awware manager of <see cref="ILogScope"/> instances.
    /// </summary>
    sealed partial class LogScopeManager : ILogScopeManager
    {
        private IExtensionManager _extensionManager;

        public LogScopeManager(IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
        }

        private ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger(typeof(LogScopeManager));

#if !NET45
        private AsyncLocal<ILogScope> _activeLogScope = new AsyncLocal<ILogScope>();

        private AsyncLocal<ILogScope> _rootLogScope = new AsyncLocal<ILogScope>();

        /// <summary>
        /// Returns current active LogScope which provides methods for logging.
        /// </summary>
        public ILogScope ActiveScope
        {
            get
            {
                if (_activeLogScope.Value == null)
                {
                    ActiveScope = RootScope;
                }

                return _activeLogScope.Value;
            }
            set
            {
                _activeLogScope.Value = value;
            }
        }

        public ILogScope RootScope
        {
            get
            {
                if (_rootLogScope.Value == null)
                {
                    TraceLogger.Info($"New log context identified, activating {typeof(RootLogScope).Name}");
                    RootScope = new RootLogScope(this, _extensionManager);
                }

                return _rootLogScope.Value;
            }
            private set
            {
                _rootLogScope.Value = value;
            }
        }
#endif

        private static Lazy<ILogScopeManager> _instance = new Lazy<ILogScopeManager>(() => new LogScopeManager(ExtensionManager.Instance));

        /// <summary>
        /// Returns instance of <see cref="ILogScopeManager"/>.
        /// </summary>
        public static ILogScopeManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}
