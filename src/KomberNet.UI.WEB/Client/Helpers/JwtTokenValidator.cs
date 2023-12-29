// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Helpers
{
    using System.IdentityModel.Tokens.Jwt;

    public static class JwtTokenValidator
    {
        public static bool HasTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadToken(token) as JwtSecurityToken;

            return jwtSecurityToken.ValidTo <= DateTime.Now.AddMinutes(1);
        }
    }
}
