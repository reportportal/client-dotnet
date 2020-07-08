using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility
{
    public interface IExtensionManager
    {
        void Explore(string path);

        IList<ILogFormatter> LogFormatters { get; }

        IList<IReportEventsObserver> ReportEventObservers { get; }

        IList<ICommandsListener> CommandsListeners { get; }
    }
}
