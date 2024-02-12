using ReportPortal.Client.Abstractions.Requests;

namespace ReportPortal.Shared.Reporter
{
    class TestLogRequestAmender : ILogRequestAmender
    {
        private ITestReporter _testReporter;

        public TestLogRequestAmender(ITestReporter testReporter)
        {
            _testReporter = testReporter;
        }

        public void Amend(CreateLogItemRequest request)
        {
            if (request.Time < _testReporter.Info.StartTime)
            {
                request.Time = _testReporter.Info.StartTime;
            }

            request.LaunchUuid = _testReporter.LaunchReporter.Info.Uuid;
            request.TestItemUuid = _testReporter.Info.Uuid;
        }
    }
}
