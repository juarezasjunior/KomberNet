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
        private readonly UserManager<TbUser> userManager;
        private readonly IDistributedCache distributedCache;
        private readonly IOptions<JwtOptions> jwtOptions;

        public TokenService(
            UserManager<TbUser> userManager,
            IDistributedCache distributedCache,
            IOptions<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.distributedCache = distributedCache;
            this.jwtOptions = jwtOptions;
        }

        public async Task<(string Token, string RefreshToken)> GenerateTokenAsync(TbUser user, CancellationToken cancellationToken)
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
            var sessionId = Guid.NewGuid().ToString();

            var claims = new List<Claim>()
            {
                new Claim(KomberNetClaims.UserId, user.Id.ToString()),
                new Claim(KomberNetClaims.FullName, user.FullName),
                new Claim(KomberNetClaims.SessionId, sessionId),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var roles = await this.userManager.GetRolesAsync(user);

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

            await this.distributedCache.SetStringAsync(string.Format(JwtCacheKeys.RefreshTokenKey, user.Email, sessionId), refreshToken, refreshTokenDistributedCacheEntryOptions);
            await this.distributedCache.SetStringAsync(
                string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, user.Email, sessionId),
                DateTime.Now.AddMinutes(this.jwtOptions.Value.JwtRefreshTokenExpiryInMinutes).ToString(),
                refreshTokenDistributedCacheEntryOptions);
            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.UserHasLogoutKey, user.Email, sessionId));
            
            return (Token: token, RefreshToken: refreshToken);
        }

        private string GenerateRandomToken() => Guid.NewGuid().ToString() + DateTime.Now.ToString("ddMMyyyyHHmmss") + Guid.NewGuid().ToString();
    }
}
