using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Throttling execution of requests.
    /// </summary>
    public interface IRequestExecutionThrottler
    {
        /// <summary>
        /// Waits until request can be executed.
        /// </summary>
        /// <returns></returns>
        Task ReserveAsync();

        /// <summary>
        /// Release one busy executer.
        /// </summary>
        void Release();
    }
}
