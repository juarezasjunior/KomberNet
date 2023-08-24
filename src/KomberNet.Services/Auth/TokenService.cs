// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class TokenService : ITokenService
    {
        private readonly UserManager<TbApplicationUser> userManager;
        private readonly IDistributedCache distributedCache;
        private readonly IOptions<JwtOptions> jwtOptions;

        public TokenService(
            UserManager<TbApplicationUser> userManager,
            IDistributedCache distributedCache,
            IOptions<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.distributedCache = distributedCache;
            this.jwtOptions = jwtOptions;
        }

        public async Task<(string Token, string RefreshToken)> GenerateTokenAsync(TbApplicationUser applicationUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validIssuer = this.jwtOptions.Value.JwtIssuer;
            var validAudience = this.jwtOptions.Value.JwtAudience;
            var tokenExpirationInMinutes = this.jwtOptions.Value.JwtExpiryInMinutes;
            var refreshTokenExpirationInMinutes = this.jwtOptions.Value.JwtRefreshTokenExpiryInMinutes;
            var secretKey = this.jwtOptions.Value.JwtSecurityKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenExpiration = DateTime.Now.AddMinutes(tokenExpirationInMinutes);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(refreshTokenExpirationInMinutes);

            var claims = new List<Claim>()
            {
                new Claim(KomberNetClaims.UserId, applicationUser.Id.ToString()),
                new Claim(KomberNetClaims.FullName, applicationUser.FullName),
                new Claim(ClaimTypes.Name, applicationUser.UserName),
                new Claim(ClaimTypes.Email, applicationUser.Email),
            };

            var roles = await this.userManager.GetRolesAsync(applicationUser);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSecurityToken = new JwtSecurityToken(
                validIssuer,
                validAudience,
                claims,
                expires: tokenExpiration,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = this.GenerateRandomToken();

            var refreshTokenDistributedCacheEntryOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = refreshTokenExpiration,
            };

            await this.distributedCache.SetStringAsync(string.Format(JwtCacheKeys.RefreshTokenKey, applicationUser.Email), refreshToken, refreshTokenDistributedCacheEntryOptions);
            await this.distributedCache.SetStringAsync(
                string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, applicationUser.Email),
                DateTime.Now.AddMinutes(this.jwtOptions.Value.JwtRefreshTokenExpiryInMinutes).ToString(),
                refreshTokenDistributedCacheEntryOptions);
            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.UserHasLogoutKey, applicationUser.Email));

            return (Token: token, RefreshToken: refreshToken);
        }

        private string GenerateRandomToken() => Guid.NewGuid().ToString() + DateTime.Now.ToString("ddMMyyyyHHmmss") + Guid.NewGuid().ToString();
    }
}
