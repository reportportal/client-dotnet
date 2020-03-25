using System;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Invokes given func.
    /// </summary>
    public class NoneRetryRequestExecuter : IRequestExecuter
    {
        private Logging.ITraceLogger TraceLogger { get; } = Logging.TraceLogManager.Instance.GetLogger<NoneRetryRequestExecuter>();

        private IRequestExecutionThrottler _concurrentThrottler;

        /// <summary>
        /// Initializes new instance of <see cref="NoneRetryRequestExecuter"/>.
        /// </summary>
        /// <param name="throttler">Limits concurrent execution of requests.</param>
        public NoneRetryRequestExecuter(IRequestExecutionThrottler throttler)
        {
            _concurrentThrottler = throttler;
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func, Action<Exception> beforeNextAttempt = null)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            T result = default(T);

            try
            {
                if (_concurrentThrottler != null)
                {
                    await _concurrentThrottler.ReserveAsync().ConfigureAwait(false);
                }
                TraceLogger.Verbose($"Invoking {func.Method.Name} method...");
                result = await func().ConfigureAwait(false);
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
            }

            return result;
        }
    }
}
