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
    using KomberNet.UI.WEB.APIClient.Auth;
    using KomberNet.UI.WEB.Client.Auth;
    using KomberNet.UI.WEB.Client.Helpers;

    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthService authService;

        public AuthHeaderHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = await this.authService.EnsureAuthenticationAsync(request.RequestUri.AbsolutePath);

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
