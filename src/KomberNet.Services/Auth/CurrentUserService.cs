// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Security.Claims;
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;

    public class CurrentUserService : BaseService, ICurrentUserService
    {
        public Guid CurrentUserId { get; private set; }

        public string CurrentUserFullName { get; private set; } = string.Empty;

        public string CurrentUserEmail { get; private set; } = string.Empty;

        public string CurrentSessionId { get; private set; } = string.Empty;

        public string GetCurrentUserNameToAudit()
        {
            return this.CurrentUserFullName;
        }

        public void SetCurrentUser(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            this.CurrentUserEmail = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            Guid.TryParse(principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.UserId)?.Value, out var userId);
            this.CurrentUserId = userId;
            this.CurrentUserFullName = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.FullName)?.Value;
            this.CurrentSessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId)?.Value;

            if (string.IsNullOrEmpty(this.CurrentUserEmail)
                || this.CurrentUserId == default
                || string.IsNullOrEmpty(this.CurrentUserFullName)
                || string.IsNullOrEmpty(this.CurrentSessionId))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }
        }
    }
}
