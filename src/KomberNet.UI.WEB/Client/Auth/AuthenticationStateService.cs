// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using System.Security.Claims;
    using Blazored.LocalStorage;
    using KomberNet.UI.WEB.Client.Helpers;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AuthenticationStateService : IAuthenticationStateService
    {
        private readonly ILocalStorageService localStorage;

        public AuthenticationStateService(
            ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public async Task<AuthenticationState> GetAuthenticationStateAsync(string token = null)
        {
            const string AuthenticationType = "jwtAuthType";

            if (string.IsNullOrEmpty(token))
            {
                token = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.AuthTokenLocalStorageKey);
            }

            if (string.IsNullOrWhiteSpace(token)
                || JwtTokenValidator.HasTokenExpired(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), AuthenticationType)));
        }
    }
}
