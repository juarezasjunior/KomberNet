// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Providers
{
    using System.Security.Claims;
    using Blazored.LocalStorage;
    using KomberNet.UI.WEB.Client.Helpers;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string AuthenticationType = "jwtAuthType";

        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationState anonymous;

        public AppAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
            this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.AuthTokenLocalStorageKey);

            return this.FillAuthenticationState(token);
        }

        public void NotifyUserAuthentication(string token) => this.NotifyAuthenticationStateChanged(Task.FromResult(this.FillAuthenticationState(token)));

        public void NotifyUserLogout() => this.NotifyAuthenticationStateChanged(Task.FromResult(this.anonymous));

        private AuthenticationState FillAuthenticationState(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return this.anonymous;
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), AuthenticationType)));
        }
    }
}
