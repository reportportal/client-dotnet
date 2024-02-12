using System.Threading.Tasks;

namespace ReportPortal.Shared.Internal.Delegating
{
    /// <summary>
    /// Throttling execution of requests.
    /// </summary>
    public interface IRequestExecutionThrottler
    {
        /// <summary>
        /// Maximum allowed concurrent executers.
        /// </summary>
        int MaxCapacity { get; }

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
