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
        /// <returns>Response with Id of created user filter.</returns>
        Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request);

        /// <inheritdoc cref="CreateAsync(CreateUserFilterRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified user filter.
        /// </summary>
        /// <param name="id">ID of filter to update.</param>
        /// <param name="request">Request with user filter for update</param>
        /// <returns>Response with info about status of updating user filter.</returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request);

        /// <inheritdoc cref="UpdateAsync(long, UpdateUserFilterRequest)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all user filters.
        /// </summary>
        /// <returns>Filtered user filters.</returns>
        Task<Content<UserFilterResponse>> GetAsync();

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="filterOption">Filter criteria for user filters.</param>
        Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption);

        /// <inheritdoc cref="GetAsync()"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<UserFilterResponse>> GetAsync(CancellationToken cancellationToken);

        /// <inheritdoc cref="GetAsync(FilterOption)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption, CancellationToken cancellationToken);

        /// <summary>
        /// Gets user filter by id.
        /// </summary>
        /// <param name="id">Id of user filter.</param>
        /// <returns>User filter with specified id.</returns>
        Task<UserFilterResponse> GetAsync(long id);

        /// <inheritdoc cref="GetAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<UserFilterResponse> GetAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified filter by id.
        /// </summary>
        /// <param name="id">Id of user filter</param>
        /// <returns>Response with info about user filter deletion status.</returns>
        Task<MessageResponse> DeleteAsync(long id);

        /// <inheritdoc cref="DeleteAsync(long)"/>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task<MessageResponse> DeleteAsync(long id, CancellationToken cancellationToken);
    }
}
