using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Reporter;
using ReportPortal.Shared.Reporter.Http;
using ReportPortal.Shared.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ReportPortal.Shared.Tests
{
    [Collection("Static")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1000:Test classes must be public", Justification = "Skip test fixture")]
    class ReportingTest
    {
        private readonly IClientService _service;

        //private readonly Service _service = new Service(new Uri("http://localhost:8080/api/v1/"), "default_personal", "26f171a9-8bb2-45e8-9e0b-cd75bb7de670");

        private readonly ITestOutputHelper _output;

        public ReportingTest(ITestOutputHelper output)
        {
            _output = output;

            var config = new Shared.Configuration.Configuration(new Dictionary<string, object>
            {
                {ConfigurationPath.ServerUrl, "https://demo.reportportal.io/api/v1" },
                {ConfigurationPath.ServerProject, "default_personal" },
                {ConfigurationPath.ServerAuthenticationUuid, "695bb79b-0419-472f-bb7c-dd1e6e932a4f" }
            });

            _service = new ClientServiceBuilder(config).Build();
        }

        [Fact]
        public async Task BigAsyncRealTree()
        {
            var extManager = new ExtensionManager();

            var launchScheduler = new LaunchReporterBuilder(_service).With(extManager);
            var launchReporter = launchScheduler.Build(1, 20, 20);

            launchReporter.Sync();

            var launch = await _service.Launch.GetAsync(launchReporter.Info.Uuid);

            await _service.Launch.DeleteAsync(launch.Id);

            _output.WriteLine(launchReporter.StatisticsCounter.ToString());
        }

        [Fact]
        public async Task BigAsyncRealTreeWithEmptySuites()
        {
            var launchScheduler = new LaunchReporterBuilder(_service);
            var launchReporter = launchScheduler.Build(10, 0, 0);

            launchReporter.Sync();

            var launch = await _service.Launch.GetAsync(launchReporter.Info.Uuid);

            await _service.Launch.DeleteAsync(launch.Id);
        }

        [Fact]
        public async Task UseExistingLaunchId()
        {
            var launchDateTime = DateTime.UtcNow;

            var launch = await _service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = "UseExistingLaunchId",
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            var config = new ConfigurationBuilder().Build();
            config.Properties["Launch:Id"] = launch.Uuid;

            var launchReporter = new LaunchReporter(_service, config, null, new ExtensionManager());
            launchReporter.Start(new StartLaunchRequest
            {
                Name = "SomeOtherName",
                StartTime = launchDateTime.AddDays(1)
            });
            launchReporter.Finish(new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            launchReporter.Sync();

            Assert.Equal(launch.Uuid, launchReporter.Info.Uuid);
            Assert.Equal(launchDateTime.ToString(), launchReporter.Info.StartTime.ToString());

            var reportedLaunch = await _service.Launch.GetAsync(launch.Uuid);
            Assert.Equal("UseExistingLaunchId", reportedLaunch.Name);
            Assert.Equal(launchDateTime.ToString(), reportedLaunch.StartTime.ToString());

            await _service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var gotLaunch = await _service.Launch.GetAsync(launchReporter.Info.Uuid);

            await _service.Launch.DeleteAsync(gotLaunch.Id);
        }

        [Fact]
        public async Task UseRerunLaunchId()
        {
            var launchDateTime = DateTime.UtcNow;
            var launchName = "UseRerunLaunchId";

            var launch = await _service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = launchName,
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            var config = new ConfigurationBuilder().Build();
            config.Properties["Launch:RerunOf"] = launch.Uuid;

            var tasks = new List<Task<LaunchReporter>>();
            for (int i = 0; i < 3; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    var r_launch = new LaunchReporter(_service, config, null, new ExtensionManager());

                    r_launch.Start(new StartLaunchRequest
                    {
                        Name = "UseRerunLaunchId",
                        StartTime = launchDateTime,
                        Mode = LaunchMode.Debug
                    });

                    for (int j = 0; j < 30; j++)
                    {
                        var r_suiteReporter = r_launch.StartChildTestReporter(new StartTestItemRequest
                        {
                            Name = $"Suite {j}",
                            StartTime = launchDateTime,
                            Type = TestItemType.Suite
                        });

                        for (int jj = 0; jj < 3; jj++)
                        {
                            var rr_suiteReporter = r_suiteReporter.StartChildTestReporter(new StartTestItemRequest
                            {
                                Name = $"Suite {jj}",
                                StartTime = launchDateTime,
                                Type = TestItemType.Suite
                            });

                            for (int k = 0; k < 0; k++)
                            {
                                var r_test = rr_suiteReporter.StartChildTestReporter(new StartTestItemRequest
                                {
                                    Name = $"Test {k}",
                                    StartTime = launchDateTime,
                                    Type = TestItemType.Step
                                });

                                for (int l = 0; l < 0; l++)
                                {
                                    r_test.Log(new CreateLogItemRequest
                                    {
                                        Level = LogLevel.Info,
                                        Text = $"Log message #{l}",
                                        Time = launchDateTime
                                    });
                                }

                                r_test.Finish(new FinishTestItemRequest
                                {
                                    EndTime = launchDateTime,
                                    Status = Status.Passed
                                });
                            }

                            rr_suiteReporter.Finish(new FinishTestItemRequest
                            {
                                EndTime = launchDateTime,
                                Status = Status.Passed
                            });
                        }

                        r_suiteReporter.Finish(new FinishTestItemRequest
                        {
                            EndTime = launchDateTime,
                            Status = Status.Passed
                        });
                    }

                    r_launch.Finish(new FinishLaunchRequest
                    {
                        EndTime = launchDateTime
                    });

                    r_launch.Sync();

                    return r_launch;
                }));
            }

            Task.WaitAll(tasks.ToArray());

            var reportedLaunch = await _service.Launch.GetAsync(launch.Uuid);
            Assert.Equal(launchName, reportedLaunch.Name);
            Assert.Equal(launchDateTime.ToString(), reportedLaunch.StartTime.ToString());

            await _service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var gotLaunch = await _service.Launch.GetAsync(reportedLaunch.Uuid);

            await _service.Launch.DeleteAsync(gotLaunch.Id);
        }

        [Fact]
        public async Task UseRerun()
        {
            var launchDateTime = DateTime.UtcNow;
            var launchName = "UseRerun";

            var launch = await _service.Launch.StartAsync(new StartLaunchRequest
            {
                Name = launchName,
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            await _service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
            {
                EndTime = DateTime.UtcNow
            });

            var config = new ConfigurationBuilder().Build();
            config.Properties["Launch:Rerun"] = true;


            var r_launch = new LaunchReporter(_service, config, null, new ExtensionManager());

            r_launch.Start(new StartLaunchRequest
            {
                Name = "UseRerun",
                StartTime = launchDateTime,
                Mode = LaunchMode.Debug
            });

            for (int j = 0; j < 1; j++)
            {
                var r_suiteReporter = r_launch.StartChildTestReporter(new StartTestItemRequest
                {
                    Name = $"Suite {j}",
                    StartTime = launchDateTime,
                    Type = TestItemType.Suite
                });

                for (int jj = 0; jj < 1; jj++)
                {
                    var rr_suiteReporter = r_suiteReporter.StartChildTestReporter(new StartTestItemRequest
                    {
                        Name = $"Suite {jj}",
                        StartTime = launchDateTime,
                        Type = TestItemType.Suite
                    });

                    for (int k = 0; k < 10; k++)
                    {
                        var r_test = rr_suiteReporter.StartChildTestReporter(new StartTestItemRequest
                        {
                            Name = $"Test {k}",
                            StartTime = launchDateTime,
                            Type = TestItemType.Step
                        });

                        for (int l = 0; l < 0; l++)
                        {
                            r_test.Log(new CreateLogItemRequest
                            {
                                Level = LogLevel.Info,
                                Text = $"Log message #{l}",
                                Time = launchDateTime
                            });
                        }

                        r_test.Finish(new FinishTestItemRequest
                        {
                            EndTime = launchDateTime,
                            Status = Status.Passed
                        });
                    }

                    rr_suiteReporter.Finish(new FinishTestItemRequest
                    {
                        EndTime = launchDateTime,
                        Status = Status.Passed
                    });
                }

                r_suiteReporter.Finish(new FinishTestItemRequest
                {
                    EndTime = launchDateTime,
                    Status = Status.Passed
                });
            }

            r_launch.Finish(new FinishLaunchRequest
            {
                EndTime = launchDateTime
            });

            r_launch.Sync();

            var reportedLaunch = await _service.Launch.GetAsync(r_launch.Info.Uuid);
            Assert.Equal(launchName, reportedLaunch.Name);
            Assert.Equal(launchDateTime.ToString(), reportedLaunch.StartTime.ToString());

            var gotLaunch = await _service.Launch.GetAsync(r_launch.Info.Uuid);

            await _service.Launch.DeleteAsync(gotLaunch.Id);
        }
    }
}
