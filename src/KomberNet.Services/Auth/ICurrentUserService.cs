﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Security.Claims;

    public interface ICurrentUserService : IScopedService
    {
        public Guid CurrentUserId { get; }

        public string CurrentUserFullName { get; }

        public string CurrentUserEmail { get; }

        public string CurrentSessionId { get; }

        public string GetCurrentUserNameToAudit();

        public void SetCurrentUser(ClaimsPrincipal principal);
    }
}