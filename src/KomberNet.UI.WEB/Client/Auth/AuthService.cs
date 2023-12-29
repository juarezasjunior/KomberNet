// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using System.Net.Http.Headers;
    using Blazored.LocalStorage;
    using KomberNet.Models.Auth;
    using KomberNet.UI.WEB.APIClient.Auth;
    using KomberNet.UI.WEB.Client.Helpers;

    public class AuthService : IAuthService
    {
        private readonly IAuthAnonymousClient authAnonymousClient;
        private readonly ILocalStorageService localStorage;

        public AuthService(
            IAuthAnonymousClient authAnonymousClient,
            ILocalStorageService localStorage)
        {
            this.authAnonymousClient = authAnonymousClient;
            this.localStorage = localStorage;
        }

        public async Task<AuthenticationHeaderValue> EnsureAuthenticationAsync(string urlPath)
        {
            var token = await this.GetValidTokenAsync(urlPath);

            return new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<string> GetValidTokenAsync(string urlPath)
        {
            var token = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.AuthTokenLocalStorageKey);

            var hasTokenExpired = JwtTokenValidator.HasTokenExpired(token);

            if (hasTokenExpired)
            {
                token = await this.RefreshTokenAsync(token);
            }

            return token;
        }

        private async Task<string> RefreshTokenAsync(string token)
        {
            var refreshToken = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.RefreshAuthTokenLocalStorageKey);

            var refreshedTokenResponse = await this.authAnonymousClient.RefreshTokenAsync(new RefreshTokenRequest()
            {
                Token = token,
                RefreshToken = refreshToken,
            });

            await this.localStorage.SetItemAsync(LocalStorageKeys.AuthTokenLocalStorageKey, refreshedTokenResponse.Token);
            await this.localStorage.SetItemAsync(LocalStorageKeys.RefreshAuthTokenLocalStorageKey, refreshedTokenResponse.RefreshToken);

            return refreshedTokenResponse.Token;
        }
    }
}
