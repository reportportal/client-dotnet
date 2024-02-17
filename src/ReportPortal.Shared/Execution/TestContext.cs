using ReportPortal.Shared.Execution.Logging;
using ReportPortal.Shared.Execution.Metadata;
using ReportPortal.Shared.Extensibility;
using System.Threading;

namespace ReportPortal.Shared.Execution
{
    /// <summary>
    /// Represents the context for a test execution.
    /// </summary>
    public class TestContext : ITestContext
    {
        private readonly IExtensionManager _extensionManager;
        private readonly CommandsSource _commadsSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestContext"/> class.
        /// </summary>
        /// <param name="extensionManager">The extension manager.</param>
        /// <param name="commandsSource">The commands source.</param>
        public TestContext(IExtensionManager extensionManager, CommandsSource commandsSource)
        {
            _extensionManager = extensionManager;
            _commadsSource = commandsSource;
            Metadata = new TestMetadataEmitter(this, _commadsSource.TestCommandsSource as TestCommandsSource);
        }

        private readonly AsyncLocal<ILogScope> _activeLogScope = new AsyncLocal<ILogScope>();
        private readonly AsyncLocal<ILogScope> _rootLogScope = new AsyncLocal<ILogScope>();

        /// <summary>
        /// Gets or sets the current active LogScope which provides methods for logging.
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
                    RootScope = new RootLogScope(this, _extensionManager, _commadsSource);
                }

                return _rootLogScope.Value;
            }
            set
            {
                _rootLogScope.Value = value;
            }
        }

        /// <summary>
        /// Gets the metadata emitter for the test context.
        /// </summary>
        public ITestMetadataEmitter Metadata { get; private set; }
    }
}