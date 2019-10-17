using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Configuration.Providers;

namespace ReportPortal.Shared.Reporter
{
    public class LaunchReporter : ILaunchReporter
    {
        private Internal.Logging.ITraceLogger TraceLogger { get; } = Internal.Logging.TraceLogManager.GetLogger<LaunchReporter>();

        private readonly Service _service;

        private readonly SemaphoreSlim _serviceConnectionsWaiter;

        public LaunchReporter(Service service)
        {
            _service = service;

            // TODO: Apply IoC with registered IConfiguration object
            var jsonPath = System.IO.Path.GetDirectoryName(new Uri(typeof(LaunchReporter).Assembly.CodeBase).LocalPath) + "/ReportPortal.config.json";
            var config = new Configuration.ConfigurationBuilder().AddJsonFile(jsonPath).AddEnvironmentVariables().Build();
            var maxServiceConnections = config.GetValue("Server:ConnectionsLimit", int.MaxValue);
            // End TODO

            _serviceConnectionsWaiter = new SemaphoreSlim(maxServiceConnections);
        }

        public LaunchReporter(Service service, string launchId) : this(service)
        {
            _isExternalLaunchId = true;

            LaunchInfo = new Launch
            {
                Id = launchId
            };
        }

        public Launch LaunchInfo { get; private set; }

        private bool _isExternalLaunchId = false;

        public Task StartTask { get; private set; }

        public void Start(StartLaunchRequest request)
        {
            TraceLogger.Verbose($"Scheduling request to start new '{request.Name}' launch in {GetHashCode()} proxy instance");

            if (StartTask != null)
            {
                var exp = new InsufficientExecutionStackException("The launch is already scheduled for starting.");
                TraceLogger.Error(exp.ToString());
                throw exp;
            }

            if (!_isExternalLaunchId)
            {
                // start new launch item
                StartTask = Task.Run(async () =>
                {
                    string launchId;

                    try
                    {
                        _serviceConnectionsWaiter.Wait();
                        launchId = (await _service.StartLaunchAsync(request)).Id;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        _serviceConnectionsWaiter.Release();
                    }
                    LaunchInfo = new Launch
                    {
                        Id = launchId,
                        Name = request.Name,
                        StartTime = request.StartTime
                    };
                });
            }
            else
            {
                // get launch info
                StartTask = Task.Run(async () =>
                {
                    try
                    {
                        _serviceConnectionsWaiter.Wait();
                        LaunchInfo = await _service.GetLaunchAsync(LaunchInfo.Id);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        _serviceConnectionsWaiter.Release();
                    }
                });
            }
        }

        public Task FinishTask { get; private set; }
        public void Finish(FinishLaunchRequest request)
        {
            TraceLogger.Verbose($"Scheduling request to finish launch in {GetHashCode()} proxy instance");

            if (StartTask == null)
            {
                var exp = new InsufficientExecutionStackException("The launch wasn't scheduled for starting to finish it properly.");
                TraceLogger.Error(exp.ToString());
                throw exp;
            }

            if (FinishTask != null)
            {
                var exp = new InsufficientExecutionStackException("The launch is already scheduled for finishing.");
                TraceLogger.Error(exp.ToString());
                throw exp;
            }

            var dependentTasks = new List<Task>();
            if (ChildTestReporters != null)
            {
                dependentTasks.AddRange(ChildTestReporters.Select(tn => tn.FinishTask));
            }
            dependentTasks.Add(StartTask);

            FinishTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (dts) =>
            {
                try
                {
                    if (StartTask.IsFaulted || StartTask.IsCanceled)
                    {
                        var exp = new Exception("Cannot finish launch due starting launch failed.", StartTask.Exception);

                        if (StartTask.IsCanceled)
                        {
                            exp = new Exception($"Cannot finish launch due {_service.Timeout} timeout while starting it.");
                        }

                        TraceLogger.Error(exp.ToString());
                        throw exp;
                    }

                    if (ChildTestReporters != null)
                    {
                        var failedChildTestReporters = ChildTestReporters.Where(ctr => ctr.FinishTask.IsFaulted || ctr.FinishTask.IsCanceled);
                        if (failedChildTestReporters.Any())
                        {
                            var errors = new List<Exception>();
                            foreach (var failedChildTestReporter in failedChildTestReporters)
                            {
                                if (failedChildTestReporter.FinishTask.IsFaulted)
                                {
                                    errors.Add(failedChildTestReporter.FinishTask.Exception);
                                }
                                else if (failedChildTestReporter.FinishTask.IsCanceled)
                                {
                                    errors.Add(new Exception($"Cannot finish launch due {_service.Timeout} timeout while finishing test item."));
                                }
                            }

                            var exp = new AggregateException("Cannot finish launch due finishing of child items failed.", errors);
                            TraceLogger.Error(exp.ToString());
                            throw exp;
                        }
                    }

                    if (request.EndTime < LaunchInfo.StartTime)
                    {
                        request.EndTime = LaunchInfo.StartTime;
                    }

                    if (!_isExternalLaunchId)
                    {
                        try
                        {
                            _serviceConnectionsWaiter.Wait();
                            await _service.FinishLaunchAsync(LaunchInfo.Id, request);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            _serviceConnectionsWaiter.Release();
                        }
                    }
                }
                finally
                {
                    // clean childs
                    // ChildTestReporters = null;
                }
            }).Unwrap();
        }

        public ConcurrentBag<ITestReporter> ChildTestReporters { get; private set; }

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, this, null, request, _serviceConnectionsWaiter);

            if (ChildTestReporters == null)
            {
                ChildTestReporters = new ConcurrentBag<ITestReporter>();
            }
            ChildTestReporters.Add(newTestNode);

            LastTestNode = newTestNode;

            return newTestNode;
        }

        public TestReporter LastTestNode { get; set; }

        public void Sync()
        {
            StartTask?.GetAwaiter().GetResult();

            FinishTask?.GetAwaiter().GetResult();
        }
    }
}
