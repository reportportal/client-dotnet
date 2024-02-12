using ReportPortal.Shared.Extensibility;

namespace ReportPortal.Shared.Execution.Logging
{
    class RootLogScope : BaseLogScope
    {
        public RootLogScope(ILogContext logContext, IExtensionManager extensionManager, CommandsSource commandsSource) : base(logContext, extensionManager, commandsSource)
        {

        }

        public override LogScopeStatus Status { get => base.Status; set { } }

        public override void Message(ILogMessage logMessage)
        {
            CommandsSource.RaiseOnLogMessageCommand(_commandsSource, Context, new Extensibility.Commands.CommandArgs.LogMessageCommandArgs(null, logMessage));
        }

        public override ILogScope BeginScope(string name)
        {
            var logScope = new LogScope(Context, _extensionManager, _commandsSource, this, null, name);

            Context.Log = logScope;

            return logScope;
        }

        public override ILogScope Root { get => this; protected set => base.Root = value; }
    }
}
