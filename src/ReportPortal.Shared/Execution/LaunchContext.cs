using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Extensibility;
using System.Threading;

namespace ReportPortal.Shared.Execution
{
    public class LaunchContext : ILaunchContext
    {
        private readonly IExtensionManager _extensionManager;

        private readonly CommandsSource _commadsSource;

        public LaunchContext(IExtensionManager extensionManager, CommandsSource commandsSource)
        {
            _extensionManager = extensionManager;
            _commadsSource = commandsSource;
        }

        private readonly AsyncLocal<ILogScope> _activeLogScope = new AsyncLocal<ILogScope>();

        private readonly AsyncLocal<ILogScope> _rootLogScope = new AsyncLocal<ILogScope>();

        /// <summary>
        /// Returns current active LogScope which provides methods for logging.
        /// </summary>
        public ILogScope Log
        {
            get
            {
                if (_activeLogScope.Value == null)
                {
                    Log = RootScope;
                }

                return _activeLogScope.Value;
            }
            set
            {
                _activeLogScope.Value = value;
            }
        }

        private ILogScope RootScope
        {
            get
            {
                if (_rootLogScope.Value == null)
                {
                    //TraceLogger.Info($"New log context identified, activating {typeof(RootLogScope).Name}");
                    RootScope = new RootLogScope(this, _extensionManager, _commadsSource);
                }

                return _rootLogScope.Value;
            }
            set
            {
                _rootLogScope.Value = value;
            }
        }
    }
}
