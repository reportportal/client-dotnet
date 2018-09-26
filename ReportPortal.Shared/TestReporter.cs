using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using System.Collections.Generic;
using ReportPortal.Client.Models;

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

        // TODO public fields :( Reworking it to properties is breaking change for agents
        public string TestId;
        public string TestName;
        public Status Status;

        public Task StartTask;
        public DateTime StartTime;
        public DateTime FinishTime;

        public int ThreadId { get; set; }

        public void Start(StartTestItemRequest request)
        {
            TestName = request.Name;

            Bridge.TestReporterExtensions.ForEach(e => e.TestNodeStarting(this, request));

            var dependentTasks = new List<Task>();
            dependentTasks.Add(_launchNode.StartTask);
            if (_parentTestNode != null)
            {
                dependentTasks.Add(_parentTestNode.StartTask);
            }

            StartTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (a) =>
            {
                try
                {
                    Task.WaitAll(dependentTasks.ToArray());
                }
                catch (Exception exp)
                {
                    var aggregatedExp = exp as AggregateException;
                    exp = aggregatedExp?.Flatten();

                    throw new Exception("Cannot start a test item due parent failed to start.", exp);
                }

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
                    if (request.StartTime < _parentTestNode.StartTime)
                    {
                        request.StartTime = _parentTestNode.StartTime;
                    }

                    TestId = (await _service.StartTestItemAsync(_parentTestNode.TestId, request)).Id;
                }

                StartTime = request.StartTime;

            }).Unwrap();
        }

        public ConcurrentBag<Task> AdditionalTasks = new ConcurrentBag<Task>();

        public Task FinishTask;
        public void Finish(FinishTestItemRequest request)
        {
            FinishTime = request.EndTime;
            Status = request.Status;

            Bridge.TestReporterExtensions.ForEach(e => e.TestNodeFinishing(this, request));

            var dependentTasks = new List<Task>();
            dependentTasks.Add(StartTask);
            dependentTasks.AddRange(AdditionalTasks);
            dependentTasks.AddRange(TestNodes.Select(tn => tn.FinishTask));

            FinishTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (a) =>
            {
                try
                {
                    StartTask.Wait();
                }
                catch (Exception exp)
                {
                    throw new Exception("Cannot finish test item due starting item failed.", exp);
                }

                try
                {
                    Task.WaitAll(TestNodes.Select(tn => tn.FinishTask).ToArray());
                }
                catch (Exception exp)
                {
                    throw new Exception("Cannot finish test item due finishing of child items failed.", exp);
                }

                if (request.EndTime < StartTime)
                {
                    request.EndTime = StartTime;
                }

                await _service.FinishTestItemAsync(TestId, request);
            }).Unwrap();
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
                AdditionalTasks.Add(StartTask.ContinueWith(async (a) =>
                {
                    await _service.UpdateTestItemAsync(TestId, request);
                }).Unwrap());
            }
        }

        public void Log(AddLogItemRequest request)
        {
            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                var dependentTasks = new List<Task>();
                dependentTasks.Add(StartTask);
                dependentTasks.AddRange(AdditionalTasks);

                var task = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (t) =>
                {
                    StartTask.Wait();

                    if (request.Time < StartTime)
                    {
                        request.Time = StartTime;
                    }

                    request.TestItemId = TestId;

                    await _service.AddLogItemAsync(request);
                }).Unwrap();

                AdditionalTasks.Add(task);
            }
        }
    }

}
