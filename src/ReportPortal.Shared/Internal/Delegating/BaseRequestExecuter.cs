using ReportPortal.Shared.Reporter.Statistics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Base class to expose functionality to execute a function (request) with statistics measuring.
    /// </summary>
    public abstract class BaseRequestExecuter : IRequestExecuter
    {
        /// <inheritdoc />
        public virtual async Task<T> ExecuteAsync<T>(Func<Task<T>> func, Action<Exception> beforeNextAttemptCallback, IStatisticsCounter statisticsCounter)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var sw = Stopwatch.StartNew();

            try
            {
                return await func();
            }
            finally
            {
                sw.Stop();

                statisticsCounter?.Measure(sw.Elapsed);
            }
        }
    }
}
