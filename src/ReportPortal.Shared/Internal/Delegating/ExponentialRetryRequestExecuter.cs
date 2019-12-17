using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Invokes given func with retry strategy and exponential delay between attempts.
    /// </summary>
    public class ExponentialRetryRequestExecuter : IRequestExecuter
    {
        private Logging.ITraceLogger TraceLogger { get; } = Logging.TraceLogManager.GetLogger<ExponentialRetryRequestExecuter>();

        private SemaphoreSlim _concurrentAwaiter;

        /// <summary>
        /// Initializes new instance of <see cref="ExponentialRetryRequestExecuter"/>.
        /// </summary>
        /// <param name="maxConcurrentRequests">Limitation of concurrent func invocation.</param>
        /// <param name="maxRetryAttempts">Maximum number of attempts.</param>
        /// <param name="baseIndex">Exponential base index for delay.</param>
        public ExponentialRetryRequestExecuter(int maxConcurrentRequests, int maxRetryAttempts, int baseIndex)
        {
            _concurrentAwaiter = new SemaphoreSlim(maxConcurrentRequests);
            MaxRetryAttemps = maxRetryAttempts;
            BaseIndex = baseIndex;
        }

        /// <summary>
        /// Maximum number of attempts
        /// </summary>
        public int MaxRetryAttemps { get; private set; }

        /// <summary>
        /// Exponential base index for delay
        /// </summary>
        public int BaseIndex { get; private set; }

        /// <inheritdoc/>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            T result = default;

            for (int i = 0; i < MaxRetryAttemps; i++)
            {
                try
                {
                    TraceLogger.Verbose($"Awaiting free executor for {func.Method.Name} method. Available: {_concurrentAwaiter.CurrentCount}");
                    await _concurrentAwaiter.WaitAsync().ConfigureAwait(false);
                    TraceLogger.Verbose($"Invoking {func.Method.Name} method... Current attempt: {i}");
                    return await func().ConfigureAwait(false);
                }
                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < MaxRetryAttemps - 1)
                    {
                        var delay = (int)Math.Pow(BaseIndex, i + MaxRetryAttemps);
                        TraceLogger.Error($"Error while invoking '{func.Method.Name}' method. Current attempt: {i}. Waiting {delay} seconds and retrying it.\n{exp}");
                        await Task.Delay(delay * 1000).ConfigureAwait(false);
                    }
                    else
                    {
                        TraceLogger.Error($"Error while invoking '{func.Method.Name}' method. Current attempt: {i}.\n{exp}");
                        throw;
                    }
                }
                catch (Exception exp)
                {
                    TraceLogger.Error($"Unexpected exception: {exp}");
                    throw;
                }
                finally
                {
                    _concurrentAwaiter.Release();
                };
            }

            return result;
        }
    }
}
