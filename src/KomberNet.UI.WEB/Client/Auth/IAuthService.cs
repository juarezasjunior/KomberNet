// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using System.Net.Http.Headers;
    using KomberNet.Models.Auth;

    public interface IAuthService
    {
        public Task<bool> LoginAsync(LoginRequest loginRequest);

        public Task LogoutAsync();

        public Task InsertUserAsync(UserInsertRequest userInsertRequest);

        public Task<AuthenticationHeaderValue> EnsureAuthenticationAsync(string urlPath);
    }
}
