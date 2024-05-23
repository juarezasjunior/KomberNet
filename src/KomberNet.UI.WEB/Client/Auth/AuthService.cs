// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using System.Net.Http.Headers;
    using Blazored.LocalStorage;
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using KomberNet.UI.WEB.APIClient.Auth;
    using KomberNet.UI.WEB.Client.Helpers;
    using KomberNet.UI.WEB.Client.Pages;
    using KomberNet.UI.WEB.Framework.Services;

    public class AuthService : IAuthService
    {
        private readonly IAuthAnonymousClient authAnonymousClient;
        private readonly ILocalStorageService localStorage;
        private readonly IInternalNavigationService internalNavigationService;
        private readonly IInternalLogoutService internalLogoutService;

        public AuthService(
            IAuthAnonymousClient authAnonymousClient,
            ILocalStorageService localStorage,
            IInternalNavigationService internalNavigationService,
            IInternalLogoutService internalLogoutService)
        {
            this.authAnonymousClient = authAnonymousClient;
            this.localStorage = localStorage;
            this.internalNavigationService = internalNavigationService;
            this.internalLogoutService = internalLogoutService;
        }

        public async Task<AuthenticationHeaderValue> EnsureAuthenticationAsync(string urlPath)
        {
            try
            {
                var token = await this.GetValidTokenAsync(urlPath);

                return new AuthenticationHeaderValue("Bearer", token);
            }
            catch
            {
                // Since we could have an exception when trying to get or refresh the token, we need to logout the user
                // The regular way of logging out is to call the logout endpoint to invalidate the refresh token
                // But, in this case, we cannot do that, because we have an exception and the user could not have a valid token anymore
                // For this reason, we just call the internal logout service
                await this.internalLogoutService.LogoutInternallyAsync();
            }

            // We still throw an exception to avoid the user to continue with the request
            throw new Exception();
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
