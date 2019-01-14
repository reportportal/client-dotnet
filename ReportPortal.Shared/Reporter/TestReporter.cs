using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Requests;
using System.Collections.Generic;
using ReportPortal.Client.Models;

namespace ReportPortal.Shared.Reporter
{
    public class TestReporter : ITestReporter
    {
        private readonly Service _service;

        public TestReporter(Service service, ILaunchReporter launchReporter, ITestReporter parentTestReporter)
        {
            _service = service;
            LaunchReporter = launchReporter;
            ParentTestReporter = parentTestReporter;

            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public TestItem TestInfo { get; } = new TestItem();

        public ILaunchReporter LaunchReporter { get; }

        public ITestReporter ParentTestReporter { get; }

        public Task StartTask { get; private set; }

        public void Start(StartTestItemRequest request)
        {
            if (StartTask != null)
            {
                throw new InsufficientExecutionStackException("The test item is already scheduled for starting.");
            }

            TestInfo.Name = request.Name;

            var dependentTasks = new List<Task>();
            dependentTasks.Add(LaunchReporter.StartTask);
            if (ParentTestReporter != null)
            {
                dependentTasks.Add(ParentTestReporter.StartTask);
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
                    if (aggregatedExp != null)
                    {
                        exp = aggregatedExp.Flatten();
                    }

                    throw new Exception("Cannot start a test item due parent failed to start.", exp);
                }

                request.LaunchId = LaunchReporter.LaunchInfo.Id;
                if (ParentTestReporter == null)
                {
                    if (request.StartTime < LaunchReporter.LaunchInfo.StartTime)
                    {
                        request.StartTime = LaunchReporter.LaunchInfo.StartTime;
                    }

                    TestInfo.Id = (await _service.StartTestItemAsync(request)).Id;
                }
                else
                {
                    if (request.StartTime < ParentTestReporter.TestInfo.StartTime)
                    {
                        request.StartTime = ParentTestReporter.TestInfo.StartTime;
                    }

                    TestInfo.Id = (await _service.StartTestItemAsync(ParentTestReporter.TestInfo.Id, request)).Id;
                }

                TestInfo.StartTime = request.StartTime;

            }).Unwrap();
        }

        public Task FinishTask { get; private set; }
        public void Finish(FinishTestItemRequest request)
        {
            if (StartTask == null)
            {
                throw new InsufficientExecutionStackException("The test item wasn't scheduled for starting to finish it properly.");
            }

            if (FinishTask != null)
            {
                throw new InsufficientExecutionStackException("The test item is already scheduled for finishing.");
            }

            TestInfo.EndTime = request.EndTime;
            TestInfo.Status = request.Status;

            var dependentTasks = new List<Task>();
            dependentTasks.Add(StartTask);
            dependentTasks.AddRange(AdditionalTasks);
            dependentTasks.AddRange(ChildTestReporters.Select(tn => tn.FinishTask));

            FinishTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (a) =>
            {
                try
                {
                    StartTask.Wait();
                }
                catch (Exception exp)
                {
                    var aggregatedExp = exp as AggregateException;
                    if (aggregatedExp != null)
                    {
                        exp = aggregatedExp.Flatten();
                    }

                    throw new Exception("Cannot finish test item due starting item failed.", exp);
                }

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

                    throw new Exception("Cannot finish test item due finishing of child items failed.", exp);
                }

                if (request.EndTime < TestInfo.StartTime)
                {
                    request.EndTime = TestInfo.StartTime;
                }

                await _service.FinishTestItemAsync(TestInfo.Id, request);
            }).Unwrap();
        }

        public ConcurrentBag<Task> AdditionalTasks = new ConcurrentBag<Task>();
        public ConcurrentBag<ITestReporter> ChildTestReporters { get; } = new ConcurrentBag<ITestReporter>();

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            var newTestNode = new TestReporter(_service, LaunchReporter, this);
            newTestNode.Start(request);
            ChildTestReporters.Add(newTestNode);

            (LaunchReporter as LaunchReporter).LastTestNode = newTestNode;

            return newTestNode;
        }

        public void Update(UpdateTestItemRequest request)
        {
            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                AdditionalTasks.Add(StartTask.ContinueWith(async (a) =>
                {
                    await _service.UpdateTestItemAsync(TestInfo.Id, request);
                }).Unwrap());
            }
        }

        public void Log(AddLogItemRequest request)
        {
            if (StartTask == null)
            {
                throw new InsufficientExecutionStackException("The test item wasn't scheduled for starting to add log messages.");
            }

            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                var dependentTasks = new List<Task>();
                dependentTasks.Add(StartTask);
                dependentTasks.AddRange(AdditionalTasks);

                var task = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async (t) =>
                {
                    StartTask.Wait();

                    if (request.Time < TestInfo.StartTime)
                    {
                        request.Time = TestInfo.StartTime;
                    }

                    request.TestItemId = TestInfo.Id;

                    await _service.AddLogItemAsync(request);
                }).Unwrap();

                AdditionalTasks.Add(task);
            }
        }

        // TODO: need remove (used by specflow only)
        public int ThreadId { get; set; }
    }

}
