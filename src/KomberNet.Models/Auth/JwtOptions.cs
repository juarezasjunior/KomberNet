// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Models.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class JwtOptions
    {
        public const string Jwt = "Jwt";

        public string JwtIssuer { get; set; } = string.Empty;

        public string JwtAudience { get; set; } = string.Empty;

        public int JwtExpiryInMinutes { get; set; }

        public int JwtRefreshTokenExpiryInMinutes { get; set; }

        public string JwtSecurityKey { get; set; } = string.Empty;
    }
}
