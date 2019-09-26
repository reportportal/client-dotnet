using ReportPortal.Shared.Internal.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ReportPortal.Shared.Tests.Internal
{
    public class LoggerTest : IDisposable
    {
        private readonly ITestOutputHelper _out;

        private string _defaultLogFilePath = $"ReportPortal.Shared.Tests.{Process.GetCurrentProcess().Id}.log";

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

            Assert.True(File.Exists(_defaultLogFilePath));
        }

        [Fact]
        public void ShouldNotAffectDefaultTraceListeners()
        {
            Assert.Single(Trace.Listeners);
        }

        [Fact]
        public void ShouldNotCaptureDefaultTrace()
        {
            TraceLogManager.GetLogger<LoggerTest>().Info("should_see_it");

            Assert.Contains("should_see_it", File.ReadAllText(_defaultLogFilePath));

            Trace.TraceInformation("should_not_see_it");

            Assert.DoesNotContain("should_not_see_it", File.ReadAllText(_defaultLogFilePath));
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("ReportPortal_TraceLevel", "");
        }
    }
}
