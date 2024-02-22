using ReportPortal.Client.Abstractions.Resources;

namespace ReportPortal.Client.Abstractions
{
    /// <summary>
    /// Interface to interact with common Report Portal services. Provides possibility to manage almost of service's endpoints.
    /// </summary>
    public interface IClientService
    {
        /// <summary>
        /// Gets the resource for managing launches.
        /// </summary>
        ILaunchResource Launch { get; }

        /// <summary>
        /// Gets the resource for managing asynchronous launches.
        /// </summary>
        IAsyncLaunchResource AsyncLaunch { get; }

        /// <summary>
        /// Gets the resource for managing test items.
        /// </summary>
        ITestItemResource TestItem { get; }

        /// <summary>
        /// Gets the resource for managing asynchronous test items.
        /// </summary>
        IAsyncTestItemResource AsyncTestItem { get; }

        /// <summary>
        /// Gets the resource for managing log items.
        /// </summary>
        ILogItemResource LogItem { get; }

        /// <summary>
        /// Gets the resource for managing asynchronous log items.
        /// </summary>
        IAsyncLogItemResource AsyncLogItem { get; }

        /// <summary>
        /// Gets the resource for managing users.
        /// </summary>
        IUserResource User { get; }

        /// <summary>
        /// Gets the resource for managing user filters.
        /// </summary>
        IUserFilterResource UserFilter { get; }

        /// <summary>
        /// Gets the resource for managing projects.
        /// </summary>
        IProjectResource Project { get; }
    }
}
