using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public class LogsReporter : ILogsReporter
    {
        private static ITraceLogger TraceLogger { get; } = TraceLogManager.Instance.GetLogger<LogsReporter>();

        private readonly Queue<CreateLogItemRequest> _buffer = new Queue<CreateLogItemRequest>();

        private readonly bool _asyncReporting;
        private readonly IReporter _reporter;
        private readonly IClientService _service;
        private readonly IConfiguration _configuration;
        private readonly IExtensionManager _extensionManager;
        private readonly IRequestExecuter _requestExecuter;
        private readonly ILogRequestAmender _logRequestAmender;

        private readonly ReportEventsSource _reportEventsSource;

        public Task ProcessingTask { get; private set; }

        public LogsReporter(IReporter testReporter,
                            IClientService service,
                            IConfiguration configuration,
                            IExtensionManager extensionManager,
                            IRequestExecuter requestExecuter,
                            ILogRequestAmender logRequestAmender,
                            ReportEventsSource reportEventsSource,
                            int batchCapacity)
        {
            _reporter = testReporter;
            _service = service;
            _configuration = configuration;
            _extensionManager = extensionManager;
            _requestExecuter = requestExecuter;
            _logRequestAmender = logRequestAmender;
            _reportEventsSource = reportEventsSource;
            _asyncReporting = _configuration.GetValue(ConfigurationPath.AsyncReporting, false);

            if (batchCapacity < 1) throw new ArgumentException("Batch capacity for logs processing cannot be less than 1.", nameof(batchCapacity));
            BatchCapacity = batchCapacity;
        }

        private readonly object _syncObj = new object();

        public int BatchCapacity { get; }

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
                        // only if parent reporter is successful
                        if (!_reporter.StartTask.IsFaulted && !_reporter.StartTask.IsCanceled)
                        {
                            var requests = GetBufferedLogRequests(batchCapacity: BatchCapacity);

                            if (requests.Count != 0)
                            {
                                foreach (var logItemRequest in requests)
                                {
                                    _logRequestAmender.Amend(logItemRequest);
                                }

                                NotifySending(requests);

                                await _requestExecuter
                                    .ExecuteAsync(async () => _asyncReporting
                                        ? await _service.AsyncLogItem.CreateAsync(requests.ToArray())
                                        : await _service.LogItem.CreateAsync(requests.ToArray()), null, _reporter.StatisticsCounter.LogItemStatisticsCounter)
                                    .ConfigureAwait(false);

                                NotifySent(requests.AsReadOnly());
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        TraceLogger.Error($"Unexpected error occurred while processing buffered log requests. {exp}");
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

        private BeforeLogsSendingEventArgs NotifySending(IList<CreateLogItemRequest> requests)
        {
            var args = new BeforeLogsSendingEventArgs(_service, _configuration, requests);
            ReportEventsSource.RaiseBeforeLogsSending(_reportEventsSource, this, args);
            return args;
        }

        private AfterLogsSentEventArgs NotifySent(IReadOnlyList<CreateLogItemRequest> requests)
        {
            var args = new AfterLogsSentEventArgs(_service, _configuration, requests);
            ReportEventsSource.RaiseAfterLogsSent(_reportEventsSource, this, args);
            return args;
        }
    }
}
