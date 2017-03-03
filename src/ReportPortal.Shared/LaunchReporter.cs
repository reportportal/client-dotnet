using System.Collections.Generic;
using System.Net;
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

            TestNodes = new List<TestReporter>();

            ServicePointManager.DefaultConnectionLimit = 10;
        }

        public string LaunchId;

        public Task StartTask;

        public void Start(StartLaunchRequest request)
        {
            StartTask = Task.Run(() => { LaunchId = _service.StartLaunch(request).Id; });
        }

        public Task FinishTask;
        public void Finish(FinishLaunchRequest request)
        {
            FinishTask = Task.Run(() =>
            {
                StartTask.Wait();

                TestNodes.ForEach(tn => tn.FinishTask.Wait());

                _service.FinishLaunch(LaunchId, request);
            });
        }

        public List<TestReporter> TestNodes { get; set; }

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
