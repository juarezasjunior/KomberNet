// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Caching.Distributed;

    public class LogoutService : BaseService, ILogoutService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IDistributedCache distributedCache;

        public LogoutService(
            ICurrentUserService currentUserService,
            IDistributedCache distributedCache)
        {
            this.currentUserService = currentUserService;
            this.distributedCache = distributedCache;
        }

        public async Task<LogoutResponse> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.LogoutSessionAsync(this.currentUserService.CurrentUserEmail, this.currentUserService.CurrentSessionId, cancellationToken);

            return new LogoutResponse();
        }

        public async Task<LogoutResponse> LogoutAllSessionsAsync(LogoutAllSessionsRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.LogoutAllSessionsAsync(this.currentUserService.CurrentUserEmail, cancellationToken);

            return new LogoutResponse();
        }

        public async Task LogoutSessionAsync(string email, string sessionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.distributedCache.SetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutKey, email, sessionId), "true");
            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.RefreshTokenKey, email, sessionId));
            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, email, sessionId));
        }

        public async Task LogoutAllSessionsAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.distributedCache.SetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, email), "true");
        }
    }
}
