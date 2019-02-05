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
            if (StartTask != null)
            {
                throw new InsufficientExecutionStackException("The launch is already scheduled for starting.");
            }

            if (!_isExternalLaunchId)
            {
                // start new launch item
                StartTask = Task.Factory.StartNew(async () =>
                {
                    var id = (await _service.StartLaunchAsync(request)).Id;

                    LaunchInfo = new Launch
                    {
                        Id = id,
                        StartTime = request.StartTime
                    };
                }).Unwrap();
            }
            else
            {
                // get launch info
                StartTask = Task.Factory.StartNew(async () =>
                {
                    LaunchInfo = await _service.GetLaunchAsync(LaunchInfo.Id);
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
                        throw new Exception("Cannot finish launch due starting launch failed.", StartTask.Exception);
                    }

                    if (ChildTestReporters?.Any(ctr => ctr.FinishTask.IsFaulted) == true)
                    {
                        throw new AggregateException("Cannot finish launch due inner items failed to finish.", ChildTestReporters.Where(ctr => ctr.FinishTask.IsFaulted).Select(ctr => ctr.FinishTask.Exception).ToArray());
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
                catch(Exception)
                {
                    throw;
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
            var newTestNode = new TestReporter(_service, this, null);
            newTestNode.Start(request);
            if (ChildTestReporters == null)
            {
                ChildTestReporters = new ConcurrentBag<ITestReporter>();
            }
            ChildTestReporters.Add(newTestNode);

            LastTestNode = newTestNode;

            return newTestNode;
        }

        public TestReporter LastTestNode { get; set; }
    }
}
