﻿// Copyright Contributors to the KomberNet project.
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
            // TODO: Handle with exceptions of JWT expiration
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
