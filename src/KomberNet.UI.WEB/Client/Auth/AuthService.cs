﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq.Dynamic.Core.Tokenizer;
    using System.Net.Http.Headers;
    using Blazored.LocalStorage;
    using KomberNet.Models.Auth;
    using KomberNet.UI.WEB.APIClient.Auth;
    using KomberNet.UI.WEB.Client.Helpers;
    using KomberNet.UI.WEB.Client.Providers;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly IAuthClient authClient;
        private readonly IAuthAnonymousClient authAnonymousClient;
        private readonly ILocalStorageService localStorage;

        public AuthService(
            AuthenticationStateProvider authenticationStateProvider,
            IAuthClient authClient,
            IAuthAnonymousClient authAnonymousClient,
            ILocalStorageService localStorage)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.authClient = authClient;
            this.authAnonymousClient = authAnonymousClient;
            this.localStorage = localStorage;
        }

        public async Task<bool> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var loginResponse = await this.authAnonymousClient.LoginAsync(loginRequest);

                await this.localStorage.SetItemAsync(LocalStorageKeys.AuthTokenLocalStorageKey, loginResponse.Token);
                await this.localStorage.SetItemAsync(LocalStorageKeys.RefreshAuthTokenLocalStorageKey, loginResponse.RefreshToken);

                ((AppAuthenticationStateProvider)this.authenticationStateProvider).NotifyUserAuthentication(loginResponse.Token);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await this.authClient.LogoutAsync(new LogoutRequest());

            await this.localStorage.RemoveItemAsync(LocalStorageKeys.AuthTokenLocalStorageKey);
            await this.localStorage.RemoveItemAsync(LocalStorageKeys.RefreshAuthTokenLocalStorageKey);

            ((AppAuthenticationStateProvider)this.authenticationStateProvider).NotifyUserLogout();
        }

        public async Task InsertUserAsync(UserInsertRequest userInsertRequest)
        {
            var response = await this.authAnonymousClient.InsertUserAsync(userInsertRequest);
        }

        public async Task<AuthenticationHeaderValue> EnsureAuthenticationAsync(string urlPath)
        {
            string token = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.AuthTokenLocalStorageKey);

            if (!urlPath.Contains("/api/Auth/RefreshToken"))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtSecurityToken.ValidTo <= DateTime.Now.AddMinutes(1))
                {
                    var refreshToken = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.RefreshAuthTokenLocalStorageKey);

                    var refreshedTokenResponse = await this.authClient.RefreshTokenAsync(new RefreshTokenRequest()
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                    });

                    await this.localStorage.SetItemAsync(LocalStorageKeys.AuthTokenLocalStorageKey, refreshedTokenResponse.Token);
                    await this.localStorage.SetItemAsync(LocalStorageKeys.RefreshAuthTokenLocalStorageKey, refreshedTokenResponse.RefreshToken);
                    token = refreshedTokenResponse.Token;
                }
            }

            return new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
