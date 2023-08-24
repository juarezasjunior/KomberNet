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
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<TbApplicationUser> userManager;
        private readonly IDistributedCache distributedCache;
        private readonly IOptions<JwtOptions> jwtOptions;
        private readonly ITokenService tokenService;

        public RefreshTokenService(
            ICurrentUserService currentUserService,
            UserManager<TbApplicationUser> userManager,
            IDistributedCache distributedCache,
            IOptions<JwtOptions> jwtOptions,
            ITokenService tokenService)
        {
            this.currentUserService = currentUserService;
            this.userManager = userManager;
            this.distributedCache = distributedCache;
            this.jwtOptions = jwtOptions;
            this.tokenService = tokenService;
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentUserId = this.currentUserService.CurrentUserId;
            var principal = this.GetPrincipalFromToken(request.Token);

            var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var applicationUser = await this.userManager.FindByEmailAsync(email);

            if (applicationUser == null
                || applicationUser.Id != currentUserId)
            {
                throw new SecurityException();
            }

            var refreshToken = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.RefreshTokenKey, applicationUser.Email));
            var refreshTokenExpiration = await this.distributedCache.GetStringAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, applicationUser.Email));

            if (string.IsNullOrEmpty(refreshToken)
                || string.IsNullOrEmpty(refreshTokenExpiration)
                || request.RefreshToken != refreshToken)
            {
                throw new SecurityException();
            }

            if (!DateTime.TryParse(refreshTokenExpiration, out var refreshTokenExpirationDateTime))
            {
                throw new SecurityException();
            }

            if (refreshTokenExpirationDateTime < DateTime.Now)
            {
                throw new SecurityException();
            }

            var result = await this.tokenService.GenerateTokenAsync(applicationUser, cancellationToken);

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
                throw new SecurityException();
            }

            return principal;
        }
    }
}
