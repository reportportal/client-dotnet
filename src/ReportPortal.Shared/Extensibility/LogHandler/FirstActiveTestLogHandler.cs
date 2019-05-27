using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Reporter;

namespace ReportPortal.Shared.Extensibility.LogHandler
{
    public class FirstActiveTestLogHandler : ILogHandler
    {
        public int Order => int.MaxValue;

        public bool Handle(AddLogItemRequest logRequest)
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

            return handled;
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
