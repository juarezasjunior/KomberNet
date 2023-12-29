// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly UserManager<TbUser> userManager;
        private readonly IDistributedCache distributedCache;
        private readonly IOptions<JwtOptions> jwtOptions;
        private readonly ITokenService tokenService;

        public RefreshTokenService(
            UserManager<TbUser> userManager,
            IDistributedCache distributedCache,
            IOptions<JwtOptions> jwtOptions,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.distributedCache = distributedCache;
            this.jwtOptions = jwtOptions;
            this.tokenService = tokenService;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var principal = this.GetPrincipalFromToken(request.Token);

            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            var userHasLogoutAllSessions = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, email));

            if (!string.IsNullOrEmpty(userHasLogoutAllSessions))
            {
                throw new KomberNetSecurityException();
            }

            var sessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId).Value;
            var user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new KomberNetSecurityException();
            }

            var refreshToken = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.RefreshTokenKey, user.Email, sessionId));
            var refreshTokenExpiration = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, user.Email, sessionId));

            if (string.IsNullOrEmpty(refreshToken)
                || string.IsNullOrEmpty(refreshTokenExpiration)
                || request.RefreshToken != refreshToken)
            {
                throw new KomberNetSecurityException();
            }

            if (!DateTime.TryParse(refreshTokenExpiration, out var refreshTokenExpirationDateTime))
            {
                throw new KomberNetSecurityException();
            }

            if (refreshTokenExpirationDateTime < DateTime.Now)
            {
                throw new KomberNetSecurityException();
            }

            var result = await this.tokenService.GenerateTokenAsync(user, cancellationToken);

            return new RefreshTokenResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var validIssuer = this.jwtOptions.Value.JwtIssuer;
            var validAudience = this.jwtOptions.Value.JwtAudience;
            var secretKey = this.jwtOptions.Value.JwtSecurityKey;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new KomberNetSecurityException();
            }

            return principal;
        }
    }
}
