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
    using KomberNet.UI.WEB.Client.Providers;
    using KomberNet.UI.WEB.Framework.Services;
    using Microsoft.AspNetCore.Components.Authorization;

    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly IAuthClient authClient;
        private readonly IAuthAnonymousClient authAnonymousClient;
        private readonly ILocalStorageService localStorage;
        private readonly IInternalLogoutService internalLogoutService;

        public UserService(
            AuthenticationStateProvider authenticationStateProvider,
            IAuthClient authClient,
            IAuthAnonymousClient authAnonymousClient,
            ILocalStorageService localStorage,
            IInternalLogoutService internalLogoutService)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.authClient = authClient;
            this.authAnonymousClient = authAnonymousClient;
            this.localStorage = localStorage;
            this.internalLogoutService = internalLogoutService;
        }

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            await this.authAnonymousClient.CreateUserAsync(request);
        }

        public async Task LoginAsync(LoginRequest loginRequest)
        {
            var loginResponse = await this.authAnonymousClient.LoginAsync(loginRequest);

            await this.localStorage.SetItemAsync(LocalStorageKeys.AuthTokenLocalStorageKey, loginResponse.Token);
            await this.localStorage.SetItemAsync(LocalStorageKeys.RefreshAuthTokenLocalStorageKey, loginResponse.RefreshToken);

            ((AppAuthenticationStateProvider)this.authenticationStateProvider).NotifyUserAuthentication(loginResponse.Token);
        }

        public async Task LogoutAsync()
        {
            // When we are logging out, we need to call the logout endpoint to invalidate the refresh token
            // Otherwise, it will be done automatically when the refresh token expires
            await this.authClient.LogoutAsync(new LogoutRequest());

            await this.internalLogoutService.LogoutInternallyAsync();
        }
    }
}
