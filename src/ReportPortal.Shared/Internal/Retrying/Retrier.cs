using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Retrying
{
    public class Retrier
    {
        private Logging.ITraceLogger TraceLogger { get; } = Logging.TraceLogManager.GetLogger<Retrier>();

        private SemaphoreSlim _concurrentAwaiter;

        public Retrier(int maxConcurrentMethods)
        {
            _concurrentAwaiter = new SemaphoreSlim(maxConcurrentMethods);
        }

        public int MaxRetries { get; } = 3;

        public async Task<T> InvokeAsync<T>(Func<Task<T>> func)
        {
            T result = default;

            for (int i = 0; i < MaxRetries; i++)
            {
                try
                {
                    TraceLogger.Verbose($"Awaiting free executor for {func.Method.Name} method. Available: {_concurrentAwaiter.CurrentCount}");
                    await _concurrentAwaiter.WaitAsync();
                    TraceLogger.Verbose($"Invoking {func.Method.Name} method... Current attempt: {i}");
                    return await func.Invoke();
                }
                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < MaxRetries - 1)
                    {
                        var delay = (int)Math.Pow(2, i + MaxRetries);
                        TraceLogger.Error($"Error while invoking '{func.Method.Name}' method. Current attempt: {i}. Waiting {delay} seconds and retrying it.\n{exp}");
                        await Task.Delay(delay * 1000);
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
