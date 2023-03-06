﻿using ReportPortal.Client.Abstractions.Resources;

namespace ReportPortal.Client.Abstractions
{
    /// <summary>
    /// Interface to interact with common Report Portal services. Provides possibility to manage almost of service's endpoints.
    /// </summary>
    public interface IClientService
    {
        ILaunchResource Launch { get; }

        IAsyncLaunchResource AsyncLaunch { get; }

        ITestItemResource TestItem { get; }

        IAsyncTestItemResource AsyncTestItem { get; }

        ILogItemResource LogItem { get; }

        IAsyncLogItemResource  AsyncLogItem { get; }

        IUserResource User { get; }

        IUserFilterResource UserFilter { get; }

        IProjectResource Project { get; }
    }
}