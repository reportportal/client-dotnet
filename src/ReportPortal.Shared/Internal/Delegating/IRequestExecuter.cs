using ReportPortal.Shared.Reporter.Statistics;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Delegate to invoke any Func.
    /// </summary>
    public interface IRequestExecuter
    {
        /// <summary>
        /// Executes func.
        /// </summary>
        /// <param name="func">Function for execution.</param>
        /// <param name="beforeNextAttemptCallback">Callback action to be invoked between attempts.</param>
        /// <param name="statisticsCounter">Statistics counter to capture requests duration.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Func<Task<T>> func, Action<Exception> beforeNextAttemptCallback, IStatisticsCounter statisticsCounter);
    }
}
