using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <inheritdoc/>
    public class RequestExecutionThrottler : IRequestExecutionThrottler, IDisposable
    {
        private Logging.ITraceLogger TraceLogger { get; } = Logging.TraceLogManager.Instance.GetLogger<RequestExecutionThrottler>();

        private readonly SemaphoreSlim _concurrentAwaiter;

        private int _waitingThreads = 0;

        /// <summary>
        /// Initializes new instance of <see cref="RequestExecutionThrottler"/>
        /// </summary>
        /// <param name="maxConcurrentRequests">Limit maximum number of concurrent requests.</param>
        public RequestExecutionThrottler(int maxConcurrentRequests)
        {
            if (maxConcurrentRequests < 1)
            {
                throw new ArgumentException("Maximum concurrent requests should be at least 1.", nameof(maxConcurrentRequests));
            }

            MaxCapacity = maxConcurrentRequests;

            _concurrentAwaiter = new SemaphoreSlim(maxConcurrentRequests);
        }

        /// <inheritdoc/>
        public int MaxCapacity { get; }

        /// <inheritdoc/>
        public async Task ReserveAsync()
        {
            TraceLogger.Verbose($"Awaiting free executor. Available: {_concurrentAwaiter.CurrentCount}, waiting: {_waitingThreads}");

            Interlocked.Increment(ref _waitingThreads);

            await _concurrentAwaiter.WaitAsync().ConfigureAwait(false);

            TraceLogger.Verbose($"Executor is reserved. Available: {_concurrentAwaiter.CurrentCount}");
        }

        /// <inheritdoc/>
        public void Release()
        {
            var previousCount = _concurrentAwaiter.Release();

            Interlocked.Decrement(ref _waitingThreads);

            TraceLogger.Verbose($"Executor is released. Available: {previousCount + 1}, waiting: {_waitingThreads}");
        }

        /// <summary>
        /// Releases all resources used by RequestExecutionThrottler.
        /// </summary>
        public void Dispose()
        {
            _concurrentAwaiter.Dispose();
        }
    }
}
