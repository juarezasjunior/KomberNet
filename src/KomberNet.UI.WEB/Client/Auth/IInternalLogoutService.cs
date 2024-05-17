// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Auth
{
    using KomberNet.UI.WEB.Framework.Services;

    public interface IInternalLogoutService : IScopedService
    {
        public Task LogoutInternallyAsync();
    }
}
