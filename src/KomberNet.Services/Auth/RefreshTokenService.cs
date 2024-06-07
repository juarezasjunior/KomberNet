// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Caching.Distributed;

    public class RefreshTokenService : BaseService, IRefreshTokenService
    {
        private readonly IUserManager userManager;
        private readonly IDistributedCache distributedCache;
        private readonly IClaimsPrincipalService claimsPrincipalService;
        private readonly ITokenService tokenService;

        public RefreshTokenService(
            IUserManager userManager,
            IDistributedCache distributedCache,
            IClaimsPrincipalService claimsPrincipalService,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.distributedCache = distributedCache;
            this.claimsPrincipalService = claimsPrincipalService;
            this.tokenService = tokenService;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var principal = this.claimsPrincipalService.GetPrincipalFromToken(request.Token);

            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            var userHasLogoutAllSessions = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, email));

            if (!string.IsNullOrEmpty(userHasLogoutAllSessions))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            var sessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId).Value;
            var user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            var refreshToken = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.RefreshTokenKey, user.Email, sessionId));
            var refreshTokenExpiration = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, user.Email, sessionId));

            if (string.IsNullOrEmpty(refreshToken)
                || string.IsNullOrEmpty(refreshTokenExpiration)
                || request.RefreshToken != refreshToken)
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            if (!DateTime.TryParse(refreshTokenExpiration, out var refreshTokenExpirationDateTime))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            if (refreshTokenExpirationDateTime < DateTime.Now)
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            var result = await this.tokenService.GenerateTokenAsync(user, cancellationToken);

            return new RefreshTokenResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
            };
        }
    }
}
