// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Caching.Distributed;

    public class LogoutEmailService : ILogoutEmailService
    {
        private readonly IDistributedCache distributedCache;

        public LogoutEmailService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task LogoutEmailAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.distributedCache.SetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutKey, email), "true");
            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.RefreshTokenKey, email));
            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, email));
        }
    }
}
