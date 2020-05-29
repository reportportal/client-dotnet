using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Internal.Delegating;

namespace ReportPortal.Shared.Reporter
{
    public class LaunchReporter : ILaunchReporter
    {
        private Internal.Logging.ITraceLogger TraceLogger { get; } = Internal.Logging.TraceLogManager.Instance.GetLogger<LaunchReporter>();

        private readonly IConfiguration _configuration;

        private readonly IClientService _service;

        private readonly IRequestExecuter _requestExecuter;

        private readonly IExtensionManager _extensionManager;

        private readonly ReportEventsSource _reportEventsSource;

        private readonly object _lockObj = new object();

        public LaunchReporter(IClientService service, IConfiguration configuration, IRequestExecuter requestExecuter, IExtensionManager extensionManager)
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

            _requestExecuter = requestExecuter ?? new RequestExecuterFactory(_configuration).Create();

            _extensionManager = extensionManager ?? throw new ArgumentNullException(nameof(extensionManager));

            _reportEventsSource = new ReportEventsSource();
            if (extensionManager.ReportEventObservers != null)
            {
                foreach (var reportEventObserver in extensionManager.ReportEventObservers)
                {
                    try
                    {
                        reportEventObserver.Initialize(_reportEventsSource);
                    }
                    catch (Exception initExp)
                    {
                        TraceLogger.Error($"Unhandled exception while initializing of {reportEventObserver.GetType().FullName}: {initExp}");
                    }
                }
            }

            // identify whether launch is already started by any external system
            var externalLaunchUuid = _configuration.GetValue<string>("Launch:Id", null);
            if (externalLaunchUuid != null)
            {
                _isExternalLaunchId = true;

                LaunchInfo = new LaunchInfo
                {
                    Uuid = externalLaunchUuid
                };
            }

            // identify whether launch should be rerun
            _rerunOfUuid = _configuration.GetValue<string>("Launch:RerunOf", null);

            _isRerun = _configuration.GetValue("Launch:Rerun", false);
        }

        public LaunchInfo LaunchInfo { get; private set; }

        private bool _isExternalLaunchId = false;
        private string _rerunOfUuid = null;
        private bool _isRerun;

        public Task StartTask { get; private set; }

        public void Start(StartLaunchRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            TraceLogger.Verbose($"Scheduling request to start new '{request.Name}' launch in {GetHashCode()} proxy instance");

            if (StartTask != null)
            {
                var exp = new InsufficientExecutionStackException("The launch is already scheduled for starting.");
                TraceLogger.Error(exp.ToString());
                throw exp;
            }

            if (_rerunOfUuid != null)
            {
                request.IsRerun = true;
                request.RerunOfLaunchUuid = _rerunOfUuid;
                // start rerun launch item
                StartTask = Task.Run(async () =>
                {
                    NotifyStarting(request);

                    var launch = await _requestExecuter.ExecuteAsync(() => _service.Launch.StartAsync(request), null).ConfigureAwait(false);

                    LaunchInfo = new LaunchInfo
                    {
                        Uuid = launch.Uuid,
                        Name = request.Name,
                        StartTime = request.StartTime
                    };

                    NotifyStarted();
                });
            }
            else if (!_isExternalLaunchId)
            {
                if (_isRerun)
                {
                    request.IsRerun = true;
                }

                // start new launch item
                StartTask = Task.Run(async () =>
                {
                    NotifyStarting(request);

                    var launch = await _requestExecuter.ExecuteAsync(() => _service.Launch.StartAsync(request), null).ConfigureAwait(false);

                    LaunchInfo = new LaunchInfo
                    {
                        Uuid = launch.Uuid,
                        Name = request.Name,
                        StartTime = request.StartTime
                    };

                    NotifyStarted();
                });
            }
            else
            {
                // get launch info
                StartTask = Task.Run(async () =>
                {
                    var launch = await _requestExecuter.ExecuteAsync(() => _service.Launch.GetAsync(LaunchInfo.Uuid), null).ConfigureAwait(false);

                    LaunchInfo = new LaunchInfo
                    {
                        Uuid = launch.Uuid,
                        Name = launch.Name,
                        StartTime = launch.StartTime
                    };
                });
            }
        }

        public Task FinishTask { get; private set; }
        public void Finish(FinishLaunchRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

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
                var childTestReporterFinishTasks = ChildTestReporters.Select(tn => tn.FinishTask);
                if (childTestReporterFinishTasks.Contains(null))
                {
                    throw new InsufficientExecutionStackException("Some of child test item(s) are not scheduled to finish yet.");
                }
                dependentTasks.AddRange(childTestReporterFinishTasks);
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
                            exp = new Exception($"Cannot finish launch due timeout while starting it.");
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
                                    errors.Add(new Exception($"Cannot finish launch due timeout while finishing test item."));
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
                        LaunchInfo.EndTime = request.EndTime;
                    }

                    if (!_isExternalLaunchId && _rerunOfUuid == null)
                    {
                        NotifyFinishing(request);

                        await _requestExecuter.ExecuteAsync(() => _service.Launch.FinishAsync(LaunchInfo.Uuid, request), null).ConfigureAwait(false);

                        NotifyFinished();
                    }
                }
                finally
                {
                    // clean childs
                    // ChildTestReporters = null;
                }
            }, TaskContinuationOptions.PreferFairness).Unwrap();
        }

        public IList<ITestReporter> ChildTestReporters { get; private set; }

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, _configuration, this, null, _requestExecuter, _extensionManager, _reportEventsSource);
            newTestNode.Start(request);

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

        private BeforeLaunchStartingEventArgs NotifyStarting(StartLaunchRequest request)
        {
            var args = new BeforeLaunchStartingEventArgs(_service, _configuration, request);
            Notify(() => ReportEventsSource.RaiseBeforeLaunchStarting(_reportEventsSource, this, args));
            return args;
        }

        private AfterLaunchStartedEventArgs NotifyStarted()
        {
            var args = new AfterLaunchStartedEventArgs(_service, _configuration);
            Notify(() => ReportEventsSource.RaiseAfterLaunchStarted(_reportEventsSource, this, args));
            return args;
        }

        private BeforeLaunchFinishingEventArgs NotifyFinishing(FinishLaunchRequest request)
        {
            var args = new BeforeLaunchFinishingEventArgs(_service, _configuration, request);
            Notify(() => ReportEventsSource.RaiseBeforeLaunchFinishing(_reportEventsSource, this, args));
            return args;
        }

        private AfterLaunchFinishedEventArgs NotifyFinished()
        {
            var args = new AfterLaunchFinishedEventArgs(_service, _configuration);
            Notify(() => ReportEventsSource.RaiseAfterLaunchFinished(_reportEventsSource, this, args));
            return args;
        }

        private void Notify(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch (Exception exp)
            {
                TraceLogger.Error($"Unhandled error while notifying launch event observers: {exp}");
            }
        }
    }
}
