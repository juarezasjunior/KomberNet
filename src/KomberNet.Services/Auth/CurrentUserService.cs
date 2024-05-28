// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Security.Claims;
    using KomberNet.Contracts;
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : BaseService, ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
{
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string UserEmail { get; private set; } = string.Empty;

        public string SessionId { get; private set; } = string.Empty;

        public void SetCurrentUser(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }

            this.UserEmail = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            Guid.TryParse(principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.UserId)?.Value, out var userId);
            this.UserId = userId;
            this.FullName = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.FullName)?.Value;
            this.SessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId)?.Value;

            if (string.IsNullOrEmpty(this.UserEmail)
                || this.UserId == default
                || string.IsNullOrEmpty(this.FullName)
                || string.IsNullOrEmpty(this.SessionId))
            {
                throw new KomberNetException(ExceptionCode.SecurityValidation);
            }
        }
    }
}
