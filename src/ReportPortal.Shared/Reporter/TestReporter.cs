using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportPortal.Client;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Internal.Logging;

namespace ReportPortal.Shared.Reporter
{
    public class TestReporter : ITestReporter
    {
        private readonly Service _service;
        private readonly IRequestExecuter _requestExecuter;

        private static ITraceLogger TraceLogger { get; } = TraceLogManager.GetLogger<TestReporter>();

        private readonly object _lockObj = new object();

        public TestReporter(Service service, ILaunchReporter launchReporter, ITestReporter parentTestReporter, StartTestItemRequest startTestItemRequest, IRequestExecuter requestExecuter)
        {
            _service = service;
            _requestExecuter = requestExecuter;
            LaunchReporter = launchReporter;
            ParentTestReporter = parentTestReporter;

            var parentStartTask = ParentTestReporter?.StartTask ?? LaunchReporter.StartTask;

            StartTask = parentStartTask.ContinueWith(async pt =>
            {
                if (pt.IsFaulted || pt.IsCanceled)
                {
                    var exp = new Exception("Cannot start test item due parent failed to start.", pt.Exception);

                    if (pt.IsCanceled)
                    {
                        exp = new Exception($"Cannot start test item due {_service.Timeout} timeout while starting parent.");
                    }

                    TraceLogger.Error(exp.ToString());
                    throw exp;
                }

                startTestItemRequest.LaunchId = LaunchReporter.LaunchInfo.Id;
                if (ParentTestReporter == null)
                {
                    if (startTestItemRequest.StartTime < LaunchReporter.LaunchInfo.StartTime)
                    {
                        startTestItemRequest.StartTime = LaunchReporter.LaunchInfo.StartTime;
                    }

                    var testModel = await _requestExecuter.ExecuteAsync(() => _service.StartTestItemAsync(startTestItemRequest)).ConfigureAwait(false);

                    TestInfo = new TestItem
                    {
                        Id = testModel.Id
                    };
                }
                else
                {
                    if (startTestItemRequest.StartTime < ParentTestReporter.TestInfo.StartTime)
                    {
                        startTestItemRequest.StartTime = ParentTestReporter.TestInfo.StartTime;
                    }

                    var testModel = await _requestExecuter.ExecuteAsync(() => _service.StartTestItemAsync(ParentTestReporter.TestInfo.Id, startTestItemRequest)).ConfigureAwait(false);

                    TestInfo = new TestItem
                    {
                        Id = testModel.Id
                    };
                }

                TestInfo.StartTime = startTestItemRequest.StartTime;
            }).Unwrap();

            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        public TestItem TestInfo { get; private set; }

        public ILaunchReporter LaunchReporter { get; }

        public ITestReporter ParentTestReporter { get; }

        public Task StartTask { get; private set; }

        public Task FinishTask { get; private set; }

        public void Finish(FinishTestItemRequest request)
        {
            TraceLogger.Verbose($"Scheduling request to finish test item in {GetHashCode()} proxy instance");

            if (StartTask == null)
            {
                var exp = new InsufficientExecutionStackException("The test item wasn't scheduled for starting to finish it properly.");
                TraceLogger.Error(exp.ToString());
                throw exp;
            }

            if (FinishTask != null)
            {
                var exp = new InsufficientExecutionStackException("The test item is already scheduled for finishing.");
                TraceLogger.Error(exp.ToString());
                throw exp;
            }

            var dependentTasks = new List<Task>();
            dependentTasks.Add(StartTask);
            if (_additionalTasks != null)
            {
                dependentTasks.AddRange(_additionalTasks);
            }
            if (ChildTestReporters != null)
            {
                dependentTasks.AddRange(ChildTestReporters.Select(tn => tn.FinishTask));
            }

            FinishTask = Task.Factory.ContinueWhenAll(dependentTasks.ToArray(), async a =>
            {
                try
                {
                    if (StartTask.IsFaulted || StartTask.IsCanceled)
                    {
                        var exp = new Exception("Cannot finish test item due starting item failed.", StartTask.Exception);

                        if (StartTask.IsCanceled)
                        {
                            exp = new Exception($"Cannot finish test item due {_service.Timeout} timeout while starting it.");
                        }

                        TraceLogger.Error(exp.ToString());
                        throw exp;
                    }

                    if (ChildTestReporters != null)
                    {
                        var failedChildTestReporters = ChildTestReporters.Where(ctr => ctr.FinishTask.IsFaulted || ctr.FinishTask.IsCanceled);
                        if (failedChildTestReporters.Any())
                        {
                            var errors = new List<Exception>();
                            foreach (var failedChildTestReporter in failedChildTestReporters)
                            {
                                if (failedChildTestReporter.FinishTask.IsFaulted)
                                {
                                    errors.Add(failedChildTestReporter.FinishTask.Exception);
                                }
                                else if (failedChildTestReporter.FinishTask.IsCanceled)
                                {
                                    errors.Add(new Exception($"{_service.Timeout} timeout while finishing child test item."));
                                }
                            }

                            var exp = new AggregateException("Cannot finish test item due finishing of child items failed.", errors);
                            TraceLogger.Error(exp.ToString());
                            throw exp;
                        }
                    }

                    TestInfo.EndTime = request.EndTime;
                    TestInfo.Status = request.Status;

                    if (request.EndTime < TestInfo.StartTime)
                    {
                        request.EndTime = TestInfo.StartTime;
                    }

                    await _requestExecuter.ExecuteAsync(() => _service.FinishTestItemAsync(TestInfo.Id, request)).ConfigureAwait(false);
                }
                finally
                {
                    // clean up childs
                    //ChildTestReporters = null;

                    // clean up addition tasks
                    _additionalTasks = null;
                }
            }).Unwrap();
        }

        private IList<Task> _additionalTasks;

        public IList<ITestReporter> ChildTestReporters { get; private set; }

        public ITestReporter StartChildTestReporter(StartTestItemRequest request)
        {
            TraceLogger.Verbose($"Scheduling request to start new '{request.Name}' test item in {GetHashCode()} proxy instance");

            var newTestNode = new TestReporter(_service, LaunchReporter, this, request, _requestExecuter);

            lock (_lockObj)
            {
                if (ChildTestReporters == null)
                {
                    lock (_lockObj)
                    {
                        ChildTestReporters = new List<ITestReporter>();
                    }
                }
                ChildTestReporters.Add(newTestNode);
            }
            (LaunchReporter as LaunchReporter).LastTestNode = newTestNode;

            return newTestNode;
        }

        public void Update(UpdateTestItemRequest request)
        {
            if (FinishTask == null || !FinishTask.IsCompleted)
            {
                lock (_lockObj)
                {
                    if (_additionalTasks == null)
                    {
                        _additionalTasks = new List<Task>();
                    }
                    _additionalTasks.Add(StartTask.ContinueWith(async a =>
                    {
                        await _requestExecuter.ExecuteAsync(() => _service.UpdateTestItemAsync(TestInfo.Id, request));
                    }).Unwrap());
                }
            }
        }

        public void Log(AddLogItemRequest request)
        {
            if (StartTask == null)
            {
                var exp = new InsufficientExecutionStackException("The test item wasn't scheduled for starting to add log messages.");
                TraceLogger.Error(exp.ToString());
                throw (exp);
            }

            if (StartTask.IsFaulted || StartTask.IsCanceled)
            {
                return;
            }

            if (FinishTask == null)
            {
                lock (_lockObj)
                {
                    if (_additionalTasks == null)
                    {
                        lock (_lockObj)
                        {
                            _additionalTasks = new List<Task>();
                        }
                    }

                    var parentTask = _additionalTasks.LastOrDefault() ?? StartTask;

                    var task = parentTask.ContinueWith(async pt =>
                    {
                        if (!StartTask.IsFaulted || !StartTask.IsCanceled)
                        {
                            if (request.Time < TestInfo.StartTime)
                            {
                                request.Time = TestInfo.StartTime;
                            }

                            request.TestItemId = TestInfo.Id;

                            foreach (var formatter in Bridge.LogFormatterExtensions)
                            {
                                formatter.FormatLog(ref request);
                            }

                            await _requestExecuter.ExecuteAsync(() => _service.AddLogItemAsync(request)).ConfigureAwait(false);
                        }
                    }).Unwrap();

                    _additionalTasks.Add(task);
                }
            }
        }

        public void Sync()
        {
            StartTask?.GetAwaiter().GetResult();

            FinishTask?.GetAwaiter().GetResult();
        }

        // TODO: need remove (used by specflow only)
        public int ThreadId { get; set; }
    }

}
