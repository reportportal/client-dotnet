using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Shared.Reporter
{
    public class LaunchReporter : ILaunchReporter
    {
        private Internal.Logging.ITraceLogger TraceLogger { get; } = Internal.Logging.TraceLogManager.GetLogger<LaunchReporter>();

        private readonly Service _service;

        public LaunchReporter(Service service)
        {
            _service = service;
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
                    var id = (await _service.StartLaunchAsync(request)).Id;

                    LaunchInfo = new Launch
                    {
                        Id = id,
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
                    LaunchInfo = await _service.GetLaunchAsync(LaunchInfo.Id);
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
                    if (StartTask.IsFaulted)
                    {
                        var exp = new Exception("Cannot finish launch due starting launch failed.", StartTask.Exception);
                        TraceLogger.Error(exp.ToString());
                        throw exp;
                    }

                    if (ChildTestReporters?.Any(ctr => ctr.FinishTask.IsFaulted) == true)
                    {
                        var exp = new AggregateException("Cannot finish launch due inner items failed to finish.", ChildTestReporters.Where(ctr => ctr.FinishTask.IsFaulted).Select(ctr => ctr.FinishTask.Exception).ToArray());
                        TraceLogger.Error(exp.ToString());
                        throw exp;
                    }

                    if (request.EndTime < LaunchInfo.StartTime)
                    {
                        request.EndTime = LaunchInfo.StartTime;
                    }

                    if (!_isExternalLaunchId)
                    {
                        await _service.FinishLaunchAsync(LaunchInfo.Id, request);
                    }
                }
                finally
                {
                    // clean childs
                    ChildTestReporters = null;
                }
            }).Unwrap();
        }

        public ConcurrentBag<ITestReporter> ChildTestReporters { get; private set; }

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, this, null, request);

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
