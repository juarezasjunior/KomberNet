// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Providers
{
    using KomberNet.UI.WEB.Client.Auth;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthenticationStateService authenticationStateService;

        public AppAuthenticationStateProvider(
            IAuthenticationStateService authService)
        {
            this.authenticationStateService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync() => await this.authenticationStateService.GetAuthenticationStateAsync();

        public void NotifyUserAuthentication(string token) => this.NotifyAuthenticationStateChanged(this.authenticationStateService.GetAuthenticationStateAsync(token));

        public void NotifyUserLogout() => this.NotifyAuthenticationStateChanged(this.authenticationStateService.GetAuthenticationStateAsync());
    }
}
