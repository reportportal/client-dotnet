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

            TestNodes = new ConcurrentBag<TestReporter>();
        }

        public string LaunchId;

        public Task StartTask;

        public void Start(StartLaunchRequest request)
        {
            StartTask = Task.Run(async () => { LaunchId = (await _service.StartLaunchAsync(request)).Id; });
        }

        public Task FinishTask;
        public void Finish(FinishLaunchRequest request, bool force = false)
        {
            FinishTask = Task.Run(async () =>
            {
                StartTask.Wait();

                TestNodes.ToList().ForEach(tn => tn.FinishTask.Wait());

                await _service.FinishLaunchAsync(LaunchId, request, force);
            });
        }

        public ConcurrentBag<TestReporter> TestNodes { get; set; }

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
