using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;

namespace ReportPortal.Shared.Reporter
{
    public class LaunchReporter : ILaunchReporter
    {
        private readonly Service _service;

        public LaunchReporter(Service service)
        {
            _service = service;
        }

        public LaunchReporter(Service service, string launchId) : this(service)
        {
            _isExternalLaunchId = true;
            LaunchInfo.Id = launchId;
        }

        public Launch LaunchInfo { get; } = new Launch();

        private bool _isExternalLaunchId = false;

        public Task StartTask { get; private set; }

        public void Start(StartLaunchRequest request)
        {
            if (StartTask != null)
            {
                throw new InsufficientExecutionStackException("The launch is already scheduled for starting.");
            }

            if (!_isExternalLaunchId)
            {
                // start new launch item
                StartTask = Task.Factory.StartNew(async () =>
                {
                    LaunchInfo.Id = (await _service.StartLaunchAsync(request)).Id;
                    LaunchInfo.StartTime = request.StartTime;
                }).Unwrap();
            }
            else
            {
                // get launch info
                StartTask = Task.Factory.StartNew(async () =>
                {
                    var launch = await _service.GetLaunchAsync(LaunchInfo.Id);
                    LaunchInfo.StartTime = launch.StartTime;
                }).Unwrap();
            }
        }

        public Task FinishTask { get; private set; }
        public void Finish(FinishLaunchRequest request)
        {
            if (StartTask == null)
            {
                throw new InsufficientExecutionStackException("The launch wasn't scheduled for starting to finish it properly.");
            }

            if (FinishTask != null)
            {
                throw new InsufficientExecutionStackException("The launch is already scheduled for finishing.");
            }

            var dependentTasks = ChildTestReporters.Select(tn => tn.FinishTask).ToList();
            dependentTasks.Add(StartTask);

            FinishTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (a) =>
            {
                try
                {
                    Task.WaitAll(ChildTestReporters.Select(tn => tn.FinishTask).ToArray());
                }
                catch (Exception exp)
                {
                    var aggregatedExp = exp as AggregateException;
                    if (aggregatedExp != null)
                    {
                        exp = aggregatedExp.Flatten();
                    }

                    throw new Exception("Cannot finish launch due inner items failed to finish.", exp);
                }
                finally
                {
                    // clean childs
                    while (!ChildTestReporters.IsEmpty)
                    {
                        ChildTestReporters.TryTake(out ITestReporter child);
                    }
                }

                if (request.EndTime < LaunchInfo.StartTime)
                {
                    request.EndTime = LaunchInfo.StartTime;
                }

                if (!_isExternalLaunchId)
                {
                    await _service.FinishLaunchAsync(LaunchInfo.Id, request);
                }
            }).Unwrap();
        }

        public ConcurrentBag<ITestReporter> ChildTestReporters { get; set; } = new ConcurrentBag<ITestReporter>();

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, this, null);
            newTestNode.Start(request);
            ChildTestReporters.Add(newTestNode);

            LastTestNode = newTestNode;

            return newTestNode;
        }

        public TestReporter LastTestNode { get; set; }
    }
}
