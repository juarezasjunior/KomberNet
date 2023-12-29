// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.APIClient.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using Refit;

    public partial interface IAuthClient : IAuthenticatedAPIClient
    {
        [Post("/api/Auth/Logout")]
        public Task<LogoutResponse> LogoutAsync([Body] LogoutRequest request);

        [Post("/api/Auth/LogoutAllSessions")]
        public Task<LogoutAllSessionsResponse> LogoutAllSessionsAsync([Body] LogoutAllSessionsRequest request);

        [Post("/api/Auth/ChangePassword")]
        public Task<ChangePasswordResponse> ChangePasswordAsync([Body] ChangePasswordRequest request);
    }
}
