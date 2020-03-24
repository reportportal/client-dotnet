using System.Linq;
using System.Threading;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Logging;
using ReportPortal.Shared.Reporter;

namespace ReportPortal.Shared.Extensibility.LogHandler
{
    public class FirstActiveTestLogHandler : ILogHandler
    {
        public int Order => int.MaxValue;

        public void BeginScope(ILogScope logScope)
        {

        }

        public void EndScope(ILogScope logScope)
        {

        }

        public bool Handle(ILogScope logScope, CreateLogItemRequest logRequest)
        {
            var handled = false;

            if ((Bridge.Context.LaunchReporter as LaunchReporter)?.LastTestNode != null)
            {
                var testNode = Bridge.Context.LaunchReporter.ChildTestReporters?
                    .Select(t => FindNonFinishedTestReporter(t, Thread.CurrentThread.ManagedThreadId))
                    .FirstOrDefault(t => t != null) ?? (Bridge.Context.LaunchReporter as LaunchReporter).LastTestNode;

                if (testNode != null)
                {
                    testNode.Log(logRequest);

                    handled = true;
                }
            }

            //return handled;
            return false;
        }

        private ITestReporter FindNonFinishedTestReporter(ITestReporter testReporter, int threadId)
        {
            if (testReporter.FinishTask == null && testReporter.ChildTestReporters == null && (testReporter as TestReporter).ThreadId == threadId)
            {
                return testReporter;
            }

            if (testReporter.ChildTestReporters != null)
            {
                return testReporter.ChildTestReporters
                  .Select(testNode => FindNonFinishedTestReporter(testNode, threadId))
                  .FirstOrDefault(t => t != null);
            }
            else
            {
                return null;
            }
        }
    }
}
