using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Reporter
{
    class LaunchLogRequestAmender : ILogRequestAmender
    {
        private ILaunchReporter _launchReporter;

        public LaunchLogRequestAmender(ILaunchReporter launchReporter)
        {
            _launchReporter = launchReporter;
        }

        public void Amend(CreateLogItemRequest request)
        {
            if (request.Time < _launchReporter.Info.StartTime)
            {
                request.Time = _launchReporter.Info.StartTime;
            }

            request.LaunchUuid = _launchReporter.Info.Uuid;
        }
    }
}
