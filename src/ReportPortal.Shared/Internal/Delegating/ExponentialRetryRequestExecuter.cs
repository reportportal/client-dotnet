using ReportPortal.Shared.Reporter.Statistics;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Invokes given func with retry strategy and exponential delay between attempts.
    /// </summary>
    public class ExponentialRetryRequestExecuter : BaseRequestExecuter
    {
        private Logging.ITraceLogger TraceLogger { get; } = Logging.TraceLogManager.Instance.GetLogger<ExponentialRetryRequestExecuter>();

        private readonly IRequestExecutionThrottler _concurrentThrottler;

        /// <summary>
        /// Initializes new instance of <see cref="ExponentialRetryRequestExecuter"/>.
        /// </summary>
        /// <param name="maxRetryAttempts">Maximum number of attempts.</param>
        /// <param name="baseIndex">Exponential base index for delay.</param>
        public ExponentialRetryRequestExecuter(uint maxRetryAttempts, uint baseIndex) :
            this(maxRetryAttempts, baseIndex, null, null)
        {

        }

        /// <summary>
        /// Initializes new instance of <see cref="ExponentialRetryRequestExecuter"/>.
        /// </summary>
        /// <param name="maxRetryAttempts">Maximum number of attempts.</param>
        /// <param name="baseIndex">Exponential base index for delay.</param>
        /// <param name="throttler">Limits concurrent execution of requests.</param>
        /// <param name="httpStatusCodes">Http status codes to be retried.</param>
        public ExponentialRetryRequestExecuter(uint maxRetryAttempts, uint baseIndex, IRequestExecutionThrottler throttler, HttpStatusCode[] httpStatusCodes)
        {
            if (maxRetryAttempts < 1)
            {
                throw new ArgumentException("Maximum attempts cannot be less than 1.", nameof(maxRetryAttempts));
            }

            _concurrentThrottler = throttler;
            MaxRetryAttemps = maxRetryAttempts;
            BaseIndex = baseIndex;
            HttpStatusCodes = httpStatusCodes;
        }

        /// <summary>
        /// Maximum number of attempts
        /// </summary>
        public uint MaxRetryAttemps { get; private set; }

        /// <summary>
        /// Exponential base index for delay
        /// </summary>
        public uint BaseIndex { get; private set; }

        /// <summary>
        /// Http status codes to be retried.
        /// </summary>
        public HttpStatusCode[] HttpStatusCodes { get; private set; }

        /// <inheritdoc/>
        public override async Task<T> ExecuteAsync<T>(Func<Task<T>> func, Action<Exception> beforeNextAttempt = null, IStatisticsCounter statisticsCounter = null)
        {
            T result = default;

            for (int i = 0; i < MaxRetryAttemps; i++)
            {
                try
                {
                    if (_concurrentThrottler != null)
                    {
                        await _concurrentThrottler.ReserveAsync().ConfigureAwait(false);
                    }

                    TraceLogger.Verbose($"Invoking {func.Method.Name} method... Current attempt: {i}");

                    result = await base.ExecuteAsync(func, beforeNextAttempt, statisticsCounter).ConfigureAwait(false);
                    break;
                }
                catch (Exception exp) when (exp is TaskCanceledException ||
                    exp is HttpRequestException ||
                    Array.IndexOf(HttpStatusCodes, (exp as Client.ServiceException)?.HttpStatusCode) > -1)
                {
                    if (i < MaxRetryAttemps - 1)
                    {
                        var delay = (int)Math.Pow(BaseIndex, i + MaxRetryAttemps);

                        TraceLogger.Error($"Error while invoking '{func.Method.Name}' method. Current attempt: {i}. Waiting {delay} seconds and retrying it.\n{exp}");

                        await Task.Delay(delay * 1000).ConfigureAwait(false);

                        beforeNextAttempt?.Invoke(exp);
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
                    if (_concurrentThrottler != null)
                    {
                        _concurrentThrottler.Release();
                    }
                };
            }

            return result;
        }
    }
}
