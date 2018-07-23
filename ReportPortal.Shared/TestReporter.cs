using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using System.Diagnostics;

namespace ReportPortal.Shared
{
    public class TestReporter
    {
        private readonly Service _service;

        private readonly LaunchReporter _launchNode;

        private readonly TestReporter _parentTestNode;

        public TestReporter(Service service, LaunchReporter launchNode, TestReporter parentTestNode)
        {
            _service = service;
            _launchNode = launchNode;
            _parentTestNode = parentTestNode;

            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public string TestId;

        public Task StartTask;
        public DateTime StartTime;

        public int ThreadId { get; set; }

        public void Start(StartTestItemRequest request)
        {
            StartTask = Task.Run(async () =>
            {
                _launchNode.StartTask.Wait();
                request.LaunchId = _launchNode.LaunchId;
                if (_parentTestNode == null)
                {
                    if (request.StartTime < _launchNode.StartTime)
                    {
                        request.StartTime = _launchNode.StartTime;
                    }

                    TestId = (await _service.StartTestItemAsync(request)).Id;
                }
                else
                {
                    _parentTestNode.StartTask.Wait();

                    if (request.StartTime < _parentTestNode.StartTime)
                    {
                        request.StartTime = _parentTestNode.StartTime;
                    }

                    TestId = (await _service.StartTestItemAsync(_parentTestNode.TestId, request)).Id;
                }

                StartTime = request.StartTime;
            });
        }

        public ConcurrentBag<Task> AdditionalTasks = new ConcurrentBag<Task>();

        public Task FinishTask;
        public void Finish(FinishTestItemRequest request)
        {
            FinishTask = Task.Run(async () =>
            {
                StartTask.Wait();

                AdditionalTasks.ToList().ForEach(at => at.Wait());

                TestNodes.ToList().ForEach(tn => tn.FinishTask.Wait());

                await _service.FinishTestItemAsync(TestId, request);
            });
        }

        public ConcurrentBag<TestReporter> TestNodes = new ConcurrentBag<TestReporter>();

        public TestReporter StartNewTestNode(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, _launchNode, this);
            newTestNode.Start(request);
            TestNodes.Add(newTestNode);

            _launchNode.LastTestNode = newTestNode;

            return newTestNode;
        }

        public void Update(UpdateTestItemRequest request)
        {
            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                AdditionalTasks.Add(Task.Run(async () =>
                {
                    StartTask.Wait();

                    await _service.UpdateTestItemAsync(TestId, request);
                }));
            }
        }

        public void Log(AddLogItemRequest request)
        {
            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                var task = Task.WhenAll(AdditionalTasks).ContinueWith(async (t) =>
                {
                    StartTask.Wait();

                    if (request.Time < StartTime)
                    {
                        request.Time = StartTime.AddMilliseconds(1);
                    }

                    request.TestItemId = TestId;
                    Debug.WriteLine($"Log message: '{request.Text}'");
                    await _service.AddLogItemAsync(request);
                });

                AdditionalTasks.Add(task);
            }
        }
    }

}
