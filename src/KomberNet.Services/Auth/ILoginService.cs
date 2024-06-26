﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Contracts;
    using KomberNet.Models.Auth;

    public interface ILoginService : ITransientService
    {
        public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
