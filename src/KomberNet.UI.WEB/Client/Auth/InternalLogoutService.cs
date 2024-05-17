// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using Blazored.LocalStorage;
    using KomberNet.UI.WEB.Client.Helpers;
    using KomberNet.UI.WEB.Client.Pages;
    using KomberNet.UI.WEB.Client.Providers;
    using KomberNet.UI.WEB.Framework.Services;
    using Microsoft.AspNetCore.Components.Authorization;

    public class InternalLogoutService : IInternalLogoutService
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorage;
        private readonly IInternalNavigationService internalNavigationService;

        public InternalLogoutService(
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage,
            IInternalNavigationService internalNavigationService)
        {
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
            this.internalNavigationService = internalNavigationService;
        }

        public async Task LogoutInternallyAsync()
        {
            await this.localStorage.RemoveItemAsync(LocalStorageKeys.AuthTokenLocalStorageKey);
            await this.localStorage.RemoveItemAsync(LocalStorageKeys.RefreshAuthTokenLocalStorageKey);

            ((AppAuthenticationStateProvider)this.authenticationStateProvider).NotifyUserLogout();

            await this.internalNavigationService.NavigateToPageAsync<Login>(forceLoad: true);
        }
    }
}
