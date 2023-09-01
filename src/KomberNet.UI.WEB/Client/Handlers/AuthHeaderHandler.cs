// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Handlers
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Headers;
    using System.Security;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Blazored.LocalStorage;
    using KomberNet.Exceptions;
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

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var securityException = JsonSerializer.Deserialize<KomberNetSecurityException>(responseContent);

                switch (securityException.ExceptionCode)
                {
                    case ExceptionCode.Others:
                        break;
                    case ExceptionCode.SecurityValidation:
                        break;
                    case ExceptionCode.InvalidPassword:
                        break;
                }
            }

            return response;
        }
    }
}
