using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Retrying
{
    public class Retrier
    {
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
                    _concurrentAwaiter.Wait();
                    return await func.Invoke();
                }
                catch (Exception exp) when (exp is TaskCanceledException || exp is HttpRequestException)
                {
                    if (i < MaxRetries - 1)
                    {
                        await Task.Delay((int)Math.Pow(2, i + MaxRetries) * 1000);
                    }
                    else
                    {
                        throw;
                    }
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
