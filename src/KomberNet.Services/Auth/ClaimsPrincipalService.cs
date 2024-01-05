// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class ClaimsPrincipalService : BaseService, IClaimsPrincipalService
    {
        private readonly IOptions<JwtOptions> jwtOptions;

        public ClaimsPrincipalService(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions;
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
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
