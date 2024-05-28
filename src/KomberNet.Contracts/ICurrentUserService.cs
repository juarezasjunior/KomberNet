// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Contracts
{
    using System.Security.Claims;

    public interface ICurrentUserService : IScopedService
    {
        public Guid UserId { get; }

        public string FullName { get; }

        public string UserEmail { get; }

        public string SessionId { get; }

        public void SetCurrentUser(ClaimsPrincipal principal);
    }
}