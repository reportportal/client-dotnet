using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Requests;

namespace ReportPortal.Shared
{
    public class LaunchReporter
    {
        private readonly Service _service;

        public LaunchReporter(Service service)
        {
            _service = service;
        }

        public LaunchReporter(Service service, string launchId) : this(service)
        {
            _isExternalLaunchId = true;
            LaunchId = launchId;
        }

        private bool _isExternalLaunchId = false;
        public string LaunchId;

        public Task StartTask;
        public DateTime StartTime;

        public void Start(StartLaunchRequest request)
        {
            if (!_isExternalLaunchId)
            {
                // start new launch item
                StartTask = Task.Factory.StartNew(async () =>
                {
                    LaunchId = (await _service.StartLaunchAsync(request)).Id;
                    StartTime = request.StartTime;
                }).Unwrap();
            }
            else
            {
                // get launch info
                StartTask = Task.Factory.StartNew(async () =>
                {
                    var launch = await _service.GetLaunchAsync(LaunchId);
                    LaunchId = launch.Id;
                    StartTime = launch.StartTime;
                }).Unwrap();
            }
        }

        public Task FinishTask;
        public void Finish(FinishLaunchRequest request, bool force = false)
        {
            var dependentTasks = TestNodes.Select(tn => tn.FinishTask).ToList();
            dependentTasks.Add(StartTask);

            FinishTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (a) =>
            {
                if (force)
                {
                    await _service.FinishLaunchAsync(LaunchId, request, force);
                }
                else
                {
                    try
                    {
                        Task.WaitAll(TestNodes.Select(tn => tn.FinishTask).ToArray());
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

                    if (request.EndTime < StartTime)
                    {
                        request.EndTime = StartTime;
                    }

                    if (!_isExternalLaunchId)
                    {
                        await _service.FinishLaunchAsync(LaunchId, request, force);
                    }
                }
            }).Unwrap();
        }

        public ConcurrentBag<TestReporter> TestNodes { get; set; } = new ConcurrentBag<TestReporter>();

        public TestReporter StartNewTestNode(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, this, null);
            newTestNode.Start(request);
            TestNodes.Add(newTestNode);

            LastTestNode = newTestNode;

            return newTestNode;
        }

        public TestReporter LastTestNode { get; set; }
    }
}
