using ReportPortal.Client.Abstractions;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Internal.Delegating;
using ReportPortal.Shared.Internal.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Reporter
{
    public class LogsReporter : ILogsReporter
    {
        private static ITraceLogger TraceLogger { get; } = TraceLogManager.Instance.GetLogger<LogsReporter>();

        private readonly BlockingCollection<CreateLogItemRequest> _queue = new BlockingCollection<CreateLogItemRequest>();

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

            ProcessingTask = _reporter.StartTask.ContinueWith(async consumer =>
            {
                await ConsumeLogRequests();
            }).Unwrap();
        }

        public int BatchCapacity { get; }

        public void Log(CreateLogItemRequest logRequest)
        {
            _queue.Add(logRequest);
        }

        public void Finish()
        {
            _queue.CompleteAdding();
        }

        public void Sync()
        {
            try
            {
                Finish();

                ProcessingTask?.GetAwaiter().GetResult();
            }
            catch
            {
                // we don't aware of failed requests for sending log messages (for now)
            }
        }

        private async Task ConsumeLogRequests()
        {
            try
            {
                foreach (var logRequest in _queue.GetConsumingEnumerable())
                {
                    if (logRequest.Attach != null)
                    {
                        await SendLogRequests(new List<CreateLogItemRequest> { logRequest });
                    }
                    else
                    {
                        var buffer = new List<CreateLogItemRequest>
                            {
                                logRequest
                            };

                        for (int i = 0; i < BatchCapacity - 1; i++)
                        {
                            if (_queue.TryTake(out var nextLogRequest))
                            {
                                if (nextLogRequest.Attach != null)
                                {
                                    await SendLogRequests(buffer);

                                    buffer.Clear();

                                    await SendLogRequests(new List<CreateLogItemRequest> { nextLogRequest });
                                }
                                else
                                {
                                    buffer.Add(nextLogRequest);
                                }
                            }
                        }

                        if (buffer.Count > 0)
                        {
                            await SendLogRequests(buffer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceLogger.Error($"Unexpected error occurred while processing buffered log requests. {ex}");
            }
        }

        private async Task SendLogRequests(List<CreateLogItemRequest> logRequests)
        {
            // only if parent reporter is successful
            if (!_reporter.StartTask.IsFaulted && !_reporter.StartTask.IsCanceled)
            {
                try
                {
                    foreach (var logItemRequest in logRequests)
                    {
                        _logRequestAmender.Amend(logItemRequest);
                    }

                    NotifySending(logRequests);

                    await _requestExecuter
                        .ExecuteAsync(async () => _asyncReporting
                            ? await _service.AsyncLogItem.CreateAsync(logRequests.ToArray())
                            : await _service.LogItem.CreateAsync(logRequests.ToArray()), null, _reporter.StatisticsCounter.LogItemStatisticsCounter)
                        .ConfigureAwait(false);

                    NotifySent(logRequests.AsReadOnly());
                }
                catch (Exception ex)
                {
                    TraceLogger.Error($"Unexpected error occurred while sending log requests. {ex}");
                }
            }
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
