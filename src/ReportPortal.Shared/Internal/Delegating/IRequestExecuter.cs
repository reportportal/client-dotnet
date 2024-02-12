using ReportPortal.Shared.Reporter.Statistics;
using System;
using System.Runtime.CompilerServices;
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
        /// <param name="logicalOperationName">Logical operation name which describes the function to be invoked.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Func<Task<T>> func, Action<Exception> beforeNextAttemptCallback, IStatisticsCounter statisticsCounter, [CallerMemberName] string logicalOperationName = null);
    }
}
