﻿using ReportPortal.Client.Abstractions.Resources;
using ReportPortal.Client.Abstractions.Responses;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReportPortal.Client.Resources
{
    class ServiceUserResource : ServiceBaseResource, IUserResource
    {
        public ServiceUserResource(HttpClient httpClient, string project) : base(httpClient, project)
        {
        }

        public Task<UserResponse> GetAsync()
        {
            return GetAsync(CancellationToken.None);
        }

        public async Task<UserResponse> GetAsync(CancellationToken cancellationToken)
        {
            return await GetAsJsonAsync<UserResponse>("user", cancellationToken).ConfigureAwait(false);
        }
    }
}
