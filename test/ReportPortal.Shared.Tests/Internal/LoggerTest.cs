using ReportPortal.Shared.Internal.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ReportPortal.Shared.Tests.Internal
{
    public class LoggerTest : IDisposable
    {
        private ITestOutputHelper _out;

        public LoggerTest(ITestOutputHelper output)
        {
            _out = output;
            Environment.SetEnvironmentVariable("ReportPortal_TraceLevel", "Information");
        }

        [Fact]
        public void ConcurrentWriters()
        {
            var tasks = new List<Task>();

            for (int i = 0; i < 20; i++)
            {
                var eventId = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    _out.WriteLine(TraceLogManager.GetLogger<LoggerTest>().GetHashCode().ToString());
                    TraceLogManager.GetLogger<LoggerTest>().Info($"my message #{eventId}");
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("ReportPortal_TraceLevel", "");
        }
    }
}
