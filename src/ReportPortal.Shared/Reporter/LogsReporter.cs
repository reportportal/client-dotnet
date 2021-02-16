using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public class LogsReporter : ILogsReporter
    {
        private readonly Queue<CreateLogItemRequest> _buffer = new Queue<CreateLogItemRequest>();
        private readonly IReporter _reporter;
        private readonly IClientService _service;
        private readonly IExtensionManager _extensionManager;
        private readonly IRequestExecuter _requestExecuter;
        private readonly ILogRequestAmender _logRequestAmender;

        private static readonly ITraceLogger _traceLogger = TraceLogManager.Instance.GetLogger<LogsReporter>();

        public Task ProcessingTask { get; private set; }

        public LogsReporter(IReporter testReporter, IClientService service, IExtensionManager extensionManager, IRequestExecuter requestExecuter, ILogRequestAmender logRequestAmender)
        {
            _reporter = testReporter;
            _service = service;
            _extensionManager = extensionManager;
            _requestExecuter = requestExecuter;
            _logRequestAmender = logRequestAmender;
        }

        private readonly object _syncObj = new object();

        public int BatchCapacity { get; set; } = 10;

        public void Log(CreateLogItemRequest logRequest)
        {
            lock (_syncObj)
            {
                _buffer.Enqueue(logRequest);

                var dependentTask = ProcessingTask ?? _reporter.StartTask;

                ProcessingTask = dependentTask.ContinueWith(async (dt) =>
                {
                    try
                    {
                        // only if parent reporter is succesfull
                        if (!_reporter.StartTask.IsFaulted && !_reporter.StartTask.IsCanceled)
                        {
                            var requests = GetBufferedLogRequests(batchCapacity: BatchCapacity);

                            if (requests.Count != 0)
                            {
                                foreach (var logItemRequest in requests)
                                {
                                    _logRequestAmender.Amend(logItemRequest);

                                    foreach (var formatter in _extensionManager.LogFormatters)
                                    {
                                        formatter.FormatLog(logItemRequest);
                                    }
                                }

                                await _requestExecuter.ExecuteAsync(() => _service.LogItem.CreateAsync(requests.ToArray()), null, _reporter.StatisticsCounter.LogItemStatisticsCounter).ConfigureAwait(false);
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        _traceLogger.Error($"Unexpected error occurred while processing buffered log requests. {exp}");
                    }
                }, TaskContinuationOptions.PreferFairness).Unwrap();
            }
        }

        public void Sync()
        {
            ProcessingTask?.GetAwaiter().GetResult();
        }

        private List<CreateLogItemRequest> GetBufferedLogRequests(int batchCapacity)
        {
            var requests = new List<CreateLogItemRequest>();

            var batchContainsItemWithAttachment = false;

            lock (_syncObj)
            {
                for (int i = 0; i < batchCapacity; i++)
                {
                    if (_buffer.Count > 0)
                    {
                        var logItemRequest = _buffer.Peek();

                        if (logItemRequest.Attach != null && batchContainsItemWithAttachment)
                        {
                            break;
                        }
                        else
                        {
                            if (logItemRequest.Attach != null)
                            {
                                batchContainsItemWithAttachment = true;
                            }

                            requests.Add(_buffer.Dequeue());
                        }
                    }
                }

            }

            return requests;
        }
    }
}
