using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Invokes given func with retry strategy and linear delay between attempts.
    /// </summary>
    public class LinearRetryRequestExecuter : IRequestExecuter
    {
        private Logging.ITraceLogger TraceLogger { get; } = Logging.TraceLogManager.GetLogger<LinearRetryRequestExecuter>();

        private SemaphoreSlim _concurrentAwaiter;

        /// <summary>
        /// Initializes new instance of <see cref="LinearRetryRequestExecuter"/>.
        /// </summary>
        /// <param name="maxConcurrentRequests">Limitation of concurrent func invocation.</param>
        /// <param name="maxRetryAttempts">Maximum number of attempts.</param>
        /// <param name="delay">Delay between ateempts (in milliseconds).</param>
        public LinearRetryRequestExecuter(int maxConcurrentRequests, int maxRetryAttempts, int delay)
        {
            if (maxRetryAttempts < 1)
            {
                throw new ArgumentException("Maximum attempts cannot be less than 1.", nameof(maxRetryAttempts));
            }

            if (delay < 0)
            {
                throw new ArgumentException("Delay cannot be less than 0.", nameof(delay));
            }

            _concurrentAwaiter = new SemaphoreSlim(maxConcurrentRequests);
            MaxRetryAttemps = maxRetryAttempts;
            Delay = delay;
        }

        /// <summary>
        /// Maximum number of attempts
        /// </summary>
        public int MaxRetryAttemps { get; private set; }

        /// <summary>
        /// How many milliseconds to wait between attempts
        /// </summary>
        public int Delay { get; private set; }

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
                    result = await func().ConfigureAwait(false);
                    break;
                }
                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < MaxRetryAttemps - 1)
                    {
                        TraceLogger.Error($"Error while invoking '{func.Method.Name}' method. Current attempt: {i}. Waiting {Delay} milliseconds and retrying it.\n{exp}");
                        await Task.Delay(Delay).ConfigureAwait(false);
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
