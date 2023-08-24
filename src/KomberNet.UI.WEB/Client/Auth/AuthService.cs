// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
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
        private readonly ILocalStorageService localStorage;

        public AuthService(
            AuthenticationStateProvider authenticationStateProvider,
            IAuthClient authClient,
            ILocalStorageService localStorage)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.authClient = authClient;
            this.localStorage = localStorage;
        }

        public async Task<bool> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var loginResponse = await this.authClient.LoginAsync(loginRequest);

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

        public async Task InsertApplicationUserAsync(ApplicationUserInsertRequest applicationUserInsertRequest)
        {
            var response = await this.authClient.InsertApplicationUserAsync(applicationUserInsertRequest);
        }
    }
}
