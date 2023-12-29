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

    public class JwtCacheKeys
    {
        public const string UserHasLogoutAllSessionsKey = "UserHasLogoutAllSessions_{0}";

        public const string UserHasLogoutKey = "UserHasLogout_{0}_Session{1}";

        public const string RefreshTokenKey = "JWTRefreshToken_{0}_Session{1}";

        public const string RefreshTokenExpirationTimeKey = "JWTRefreshTokenExp_{0}_Session{1}";
    }
}
