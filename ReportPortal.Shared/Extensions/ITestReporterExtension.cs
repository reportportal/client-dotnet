using ReportPortal.Client.Requests;

namespace ReportPortal.Shared.Extensions
{
    public interface ITestReporterExtension
    {
        void TestNodeStarting(TestReporter testReporter, StartTestItemRequest request);

        void TestNodeFinishing(TestReporter testReporter, FinishTestItemRequest request);
    }
}
