using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Tests.Helpers;
using System;
using Xunit;

namespace ReportPortal.Shared.Tests.Faked
{
    public class BigTreeInvocation
    {
        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(5, 10000, 0)]
        [InlineData(5, 10000, 10)]
        public void SuccessReporting(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var fakeService = new FakeService(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");

            var launchScheduler = new LaunchScheduler(fakeService);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.FinishTask.Wait();

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.StartTestItemCounter);

            Assert.Equal(suitesPerLaunch * testsPerSuite * logsPerTest, fakeService.AddLogItemCounter);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 10, 10)]
        public void FailedLogsShouldNotAffectFinishingLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var fakeService = new FakeServiceWithFailedAddLogItemMethod(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");

            var launchScheduler = new LaunchScheduler(fakeService);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            launchReporter.FinishTask.Wait();

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.StartTestItemCounter);

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.FinishTestItemCounter);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 100, 0)]
        public void FailedFirstFinishTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var fakeService = new FakeServiceWithFailedFirstFinishTestItemMethod(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");

            var launchScheduler = new LaunchScheduler(fakeService);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.FinishTask.Wait());

            Assert.Equal(suitesPerLaunch * testsPerSuite + suitesPerLaunch, fakeService.StartTestItemCounter);

            Assert.Equal(suitesPerLaunch * testsPerSuite - 1, fakeService.FinishTestItemCounter);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 100, 0)]
        public void FailedFirstStartTestItemShouldRaiseExceptionAtFinishLaunch(int suitesPerLaunch, int testsPerSuite, int logsPerTest)
        {
            var fakeService = new FakeServiceWithFailedFirstStartTestItemMethod(new Uri("https://rp.epam.com/api/v1/"), "ci-agents-checks", "7853c7a9-7f27-43ea-835a-cab01355fd17");

            var launchScheduler = new LaunchScheduler(fakeService);
            var launchReporter = launchScheduler.Build(suitesPerLaunch, testsPerSuite, logsPerTest);

            var exp = Assert.ThrowsAny<Exception>(() => launchReporter.FinishTask.Wait());

            Assert.Equal(suitesPerLaunch - 1, fakeService.StartTestItemCounter);
        }
    }
}
