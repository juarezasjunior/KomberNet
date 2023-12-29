// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using System.Net.Http.Headers;
    using KomberNet.Models.Auth;
    using KomberNet.UI.WEB.Framework.Services;
    using Microsoft.AspNetCore.Components.Authorization;

    public interface IAuthenticationStateService : IScopedService
    {
        public Task<AuthenticationState> GetAuthenticationStateAsync(string token = null);
    }
}
