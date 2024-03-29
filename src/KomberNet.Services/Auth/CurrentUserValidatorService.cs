﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Caching.Distributed;

    public class CurrentUserValidatorService : BaseService, ICurrentUserValidatorService
    {
        private readonly IDistributedCache distributedCache;

        public CurrentUserValidatorService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task ValidateAsync(string email, string sessionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(email))
            {
                throw new KomberNetSecurityException();
            }

            var userHasLogout = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutKey, email, sessionId));

            if (!string.IsNullOrEmpty(userHasLogout))
            {
                throw new KomberNetSecurityException();
            }

            var userHasLogoutAllSessions = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, email));

            if (!string.IsNullOrEmpty(userHasLogoutAllSessions))
            {
                throw new KomberNetSecurityException();
            }
        }
    }
}
