// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using KomberNet.Contracts;
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Caching.Distributed;

    public class CurrentUserValidatorService : BaseService, ICurrentUserValidatorService
    {
        private readonly IDistributedCache distributedCache;
        private readonly ICurrentUserService currentUserService;

        public CurrentUserValidatorService(
            IDistributedCache distributedCache,
            ICurrentUserService currentUserService)
        {
            this.distributedCache = distributedCache;
            this.currentUserService = currentUserService;
        }

        public async Task ValidateAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userEmail = this.currentUserService.UserEmail;
            var userId = this.currentUserService.UserId;
            var fullName = this.currentUserService.FullName;
            var sessionId = this.currentUserService.SessionId;

            if (string.IsNullOrEmpty(userEmail)
                || userId == default
                || string.IsNullOrEmpty(fullName)
                || string.IsNullOrEmpty(sessionId))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            var userHasLogout = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutKey, userEmail, sessionId));
            if (!string.IsNullOrEmpty(userHasLogout))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            var userHasLogoutAllSessions = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, userEmail));
            if (!string.IsNullOrEmpty(userHasLogoutAllSessions))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }
        }
    }
}
