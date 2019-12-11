using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Configuration.Providers;
using ReportPortal.Shared.Internal.Delegating;

namespace ReportPortal.Shared.Reporter
{
    public class LaunchReporter : ILaunchReporter
    {
        private Internal.Logging.ITraceLogger TraceLogger { get; } = Internal.Logging.TraceLogManager.GetLogger<LaunchReporter>();

        private readonly IConfiguration _configuration;

        private readonly Service _service;

        private readonly IRequestExecuter _requestExecuter;

        private readonly object _lockObj = new object();

        [Obsolete("This ctor will be removed. Use (Service service, IConfiguration configuration, IRequestExecuter requestExecuter)")]
        public LaunchReporter(Service service) : this(service, null, null)
        {
            _service = service;
        }

        [Obsolete("This ctor will be removed. Use (Service service, IConfiguration configuration, IRequestExecuter requestExecuter) where launchId argument can be spicified by IConfiguration [Launch:Id]")]
        public LaunchReporter(Service service, string launchId) : this(service)
        {
            _isExternalLaunchId = true;

            LaunchInfo = new Launch
            {
                Id = launchId
            };
        }

        public LaunchReporter(Service service, IConfiguration configuration, IRequestExecuter requestExecuter)
        {
            _service = service;

            if (configuration != null)
            {
                _configuration = configuration;
            }
            else
            {
                var jsonPath = System.IO.Path.GetDirectoryName(new Uri(typeof(LaunchReporter).Assembly.CodeBase).LocalPath) + "/ReportPortal.config.json";
                _configuration = new ConfigurationBuilder().AddJsonFile(jsonPath).AddEnvironmentVariables().Build();
            }

            if (requestExecuter != null)
            {
                _requestExecuter = requestExecuter;
            }
            else
            {
                _requestExecuter = new RequestExecuterFactory().Create(_configuration);
            }

            // identify whether launch is already started by any external system
            var externalLaunchId = _configuration.GetValue<string>("Launch:Id", null);
            if (externalLaunchId != null)
            {
                _isExternalLaunchId = true;

                LaunchInfo = new Launch
                {
                    Id = externalLaunchId
                };
            }
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

                    var launch = await _requestExecuter.ExecuteAsync(() => _service.StartLaunchAsync(request)).ConfigureAwait(false);
                    launchId = launch.Id;

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
                    LaunchInfo = await _requestExecuter.ExecuteAsync(() => _service.GetLaunchAsync(LaunchInfo.Id)).ConfigureAwait(false);
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
                        await _requestExecuter.ExecuteAsync(() => _service.FinishLaunchAsync(LaunchInfo.Id, request)).ConfigureAwait(false);
                    }
                }
                finally
                {
                    // clean childs
                    // ChildTestReporters = null;
                }
            }).Unwrap();
        }

        public IList<ITestReporter> ChildTestReporters { get; private set; }

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, this, null, request, _requestExecuter);

            lock (_lockObj)
            {
                if (ChildTestReporters == null)
                {
                    lock (_lockObj)
                    {
                        ChildTestReporters = new List<ITestReporter>();
                    }
                }
                ChildTestReporters.Add(newTestNode);
            }

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
