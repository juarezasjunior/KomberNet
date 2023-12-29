﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;

    public interface ILogoutService : IService
    {
        public Task<LogoutResponse> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);

        public Task<LogoutResponse> LogoutAllSessionsAsync(LogoutAllSessionsRequest request, CancellationToken cancellationToken);

        public Task LogoutSessionAsync(string email, string sessionId, CancellationToken cancellationToken);

        public Task LogoutAllSessionsAsync(string email, CancellationToken cancellationToken);
    }
}
