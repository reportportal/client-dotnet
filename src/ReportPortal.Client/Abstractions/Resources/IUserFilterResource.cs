using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Abstractions.Resources
{
    /// <summary>
    /// Interacts with user filters.
    /// </summary>
    public interface IUserFilterResource
    {
        /// <summary>
        /// Creates the specified user filter.
        /// </summary>
        /// <param name="request">Request with user filter for create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response with Id of created user filter.</returns>
        Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified user filter.
        /// </summary>
        /// <param name="id">ID of filter to update.</param>
        /// <param name="request">Request with user filter for update</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response with info about status of updating user filter.</returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all user filters.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Filtered user filters.</returns>
        Task<Content<UserFilterResponse>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all user filters.
        /// </summary>
        /// /// <param name="filterOption">Filter criteria for user filters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Filtered user filters.</returns>
        Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets user filter by id.
        /// </summary>
        /// <param name="id">Id of user filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>User filter with specified id.</returns>
        Task<UserFilterResponse> GetAsync(long id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified filter by id.
        /// </summary>
        /// <param name="id">Id of user filter</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response with info about user filter deletion status.</returns>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
