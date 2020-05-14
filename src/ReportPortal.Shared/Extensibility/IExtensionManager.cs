using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility
{
    public interface IExtensionManager
    {
        void Explore(string path);

        IList<ILogFormatter> LogFormatters { get; }

        IList<ILogHandler> LogHandlers { get; }
    }
}
