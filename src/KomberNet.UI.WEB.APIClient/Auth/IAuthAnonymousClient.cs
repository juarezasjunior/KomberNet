﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.APIClient.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using Refit;

    public partial interface IAuthAnonymousClient : IAnonymousAPIClient
    {
        [Post("/api/Auth/CreateUser")]
        public Task<CreateUserResponse> CreateUserAsync([Body] CreateUserRequest request);

        [Post("/api/Auth/Login")]
        public Task<LoginResponse> LoginAsync([Body] LoginRequest request);

        [Post("/api/Auth/RefreshToken")]
        public Task<RefreshTokenResponse> RefreshTokenAsync([Body] RefreshTokenRequest request);
    }
}
