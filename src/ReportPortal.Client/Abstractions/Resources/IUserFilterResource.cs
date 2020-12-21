using ReportPortal.Client.Abstractions.Filtering;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Responses;
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
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserFilterCreatedResponse> CreateAsync(CreateUserFilterRequest request);

        /// <summary>
        /// Updates the specified user filter.
        /// </summary>
        /// <param name="id">ID of filter to update.</param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<MessageResponse> UpdateAsync(long id, UpdateUserFilterRequest request);

        /// <summary>
        /// Gets all user filters.
        /// </summary>
        /// <param name="filterOption"></param>
        /// <returns></returns>
        Task<Content<UserFilterResponse>> GetAsync(FilterOption filterOption = null);

        /// <summary>
        /// Gets user filter by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserFilterResponse> GetAsync(long id);

        /// <summary>
        /// Deletes the specified filter by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessageResponse> DeleteAsync(long id);
    }
}
