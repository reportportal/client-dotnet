using ReportPortal.Shared.Execution;
using System;

namespace ReportPortal.Shared
{
    /// <summary>
    /// Provides an access to work with reporting context.
    /// Using it you are able to add log messages, amend curent test metainfo.
    /// </summary>
    public static class Context
    {
        private static readonly Lazy<CommandsSource> _commandsSource = new Lazy<CommandsSource>(() => new CommandsSource(Extensibility.ExtensionManager.Instance.CommandsListeners));

        private static readonly Lazy<ITestContext> _current = new Lazy<ITestContext>(() => new TestContext(Extensibility.ExtensionManager.Instance, _commandsSource.Value));

        private static readonly Lazy<ILaunchContext> _launch = new Lazy<ILaunchContext>(() => new LaunchContext(Extensibility.ExtensionManager.Instance, _commandsSource.Value));

        /// <summary>
        /// Returns context to amend current test metainfo.
        /// </summary>
        public static ITestContext Current
        {
            get
            {
                return _current.Value;
            }
        }

        /// <summary>
        /// Returns context to amend current launch metainfo.
        /// </summary>
        public static ILaunchContext Launch
        {
            get
            {
                return _launch.Value;
            }
        }
    }
}
