using System.Collections.Generic;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Requests;

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

        public void Start(StartTestItemRequest request)
        {
            StartTask = Task.Run(() =>
            {
                _launchNode.StartTask.Wait();
                request.LaunchId = _launchNode.LaunchId;
                if (_parentTestNode == null)
                {
                    TestId = _service.StartTestItem(request).Id;
                }
                else
                {
                    _parentTestNode.StartTask.Wait();
                    TestId = _service.StartTestItem(_parentTestNode.TestId, request).Id;
                }
            });
        }

        public List<Task> AdditionalTasks = new List<Task>();

        public Task FinishTask;
        public void Finish(FinishTestItemRequest request)
        {
            FinishTask = Task.Run(() =>
            {
                StartTask.Wait();

                AdditionalTasks.ForEach(at => at.Wait());

                TestNodes.ForEach(tn => tn.FinishTask.Wait());

                _service.FinishTestItem(TestId, request);
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
                AdditionalTasks.Add(Task.Run(() =>
                {
                    StartTask.Wait();

                    _service.UpdateTestItem(TestId, request);
                }));
            }
        }

        public void Log(AddLogItemRequest request)
        {
            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                AdditionalTasks.Add(Task.Run(() =>
                {
                    StartTask.Wait();
                    request.TestItemId = TestId;
                    _service.AddLogItem(request);
                }));
            }
        }
    }
}
