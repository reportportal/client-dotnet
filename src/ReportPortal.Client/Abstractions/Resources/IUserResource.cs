using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Interacts with current user.
    /// </summary>
    public interface IUserResource
    {
        /// <summary>
        /// Gets the current user's information.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        Task<UserResponse> GetAsync(CancellationToken cancellationToken = default);
    }
}
