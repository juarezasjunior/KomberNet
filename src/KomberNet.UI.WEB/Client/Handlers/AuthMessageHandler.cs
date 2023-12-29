// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Handlers
{
    using KomberNet.UI.WEB.Client.Auth;

    public class AuthMessageHandler : MessageHandler
    {
        private readonly IAuthService authService;

        public AuthMessageHandler(IAuthService authService)
        {
            this.authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = await this.authService.EnsureAuthenticationAsync(request.RequestUri.AbsolutePath);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
