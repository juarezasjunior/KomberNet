// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Handlers
{
    using System.Net.Http.Headers;
    using Blazored.LocalStorage;
    using KomberNet.UI.WEB.Client.Auth;
    using KomberNet.UI.WEB.Client.Helpers;

    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorage;

        public AuthHeaderHandler(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await this.localStorage.GetItemAsync<string>(LocalStorageKeys.AuthTokenLocalStorageKey);

            if (token != null)
            {
                // TODO: See it later
                // potentially refresh token here if it has expired etc.
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
