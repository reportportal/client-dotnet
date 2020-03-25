using ReportPortal.Shared.Internal.Logging;
using System.Threading;

namespace ReportPortal.Shared.Logging
{
    /// <summary>
    /// Context awware manager of <see cref="ILogScope"/> instances.
    /// </summary>
    sealed class LogScopeManager : ILogScopeManager
    {
        private ITraceLogger TraceLogger => TraceLogManager.Instance.GetLogger(typeof(LogScopeManager));

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
                    RootScope = new RootLogScope(this);
                }

                return _rootLogScope.Value;
            }
            private set
            {
                _rootLogScope.Value = value;
            }
        }

        private static readonly object s_lockObj = new object();

        private static ILogScopeManager _current;

        /// <summary>
        /// Returns current instance of <see cref="ILogScopeManager"/>.
        /// </summary>
        public static ILogScopeManager Current
        {
            get
            {
                if (_current == null)
                {
                    lock (s_lockObj)
                    {
                        if (_current == null)
                        {
                            _current = new LogScopeManager();
                        }
                    }
                }

                return _current;
            }
        }
    }
}
