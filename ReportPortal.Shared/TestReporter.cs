using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using System;

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
        }

        public string TestId;

        public Task StartTask;
        public DateTime StartTime;

        public void Start(StartTestItemRequest request)
        {
            StartTask = Task.Run(async () =>
            {
                _launchNode.StartTask.Wait();
                request.LaunchId = _launchNode.LaunchId;
                if (_parentTestNode == null)
                {
                    TestId = (await _service.StartTestItemAsync(request)).Id;
                }
                else
                {
                    _parentTestNode.StartTask.Wait();
                    TestId = (await _service.StartTestItemAsync(_parentTestNode.TestId, request)).Id;
                }

                StartTime = request.StartTime;
            });
        }

        public List<Task> AdditionalTasks = new List<Task>();

        public Task FinishTask;
        public void Finish(FinishTestItemRequest request)
        {
            FinishTask = Task.Run(async () =>
            {
                StartTask.Wait();

                AdditionalTasks.ForEach(at => at.Wait());

                TestNodes.ForEach(tn => tn.FinishTask.Wait());

                await _service.FinishTestItemAsync(TestId, request);
            });
        }

        public List<TestReporter> TestNodes = new List<TestReporter>();

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
                AdditionalTasks.Add(Task.Run(async () =>
                {
                    StartTask.Wait();

                    if (request.Time < StartTime)
                    {
                        request.Time = StartTime.AddMilliseconds(1);
                    }

                    request.TestItemId = TestId;
                    await _service.AddLogItemAsync(request);
                }));
            }
        }
    }
}
