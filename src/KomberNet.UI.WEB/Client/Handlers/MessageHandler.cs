// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Handlers
{
    using System.Text.Json;
    using KomberNet.Exceptions;
    using KomberNet.UI.WEB.Client.Auth;

    public class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
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
