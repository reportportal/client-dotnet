using ReportPortal.Shared.Extensibility.ReportEvents;

namespace ReportPortal.Shared.Extensibility
{
    public interface IReportEventsObserver
    {
        void Initialize(IReportEventsSource reportEventsSource);
    }


}
