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

        public Guid UserId
        {
            get
            {
                if (this.ClaimsPrincipal != null)
                {
                    Guid.TryParse(this.ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.UserId)?.Value, out var userId);
                    return userId;
                }

                return default;
            }
        }

        public string FullName
        {
            get
            {
                if (this.ClaimsPrincipal != null)
                {
                    return this.ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.FullName)?.Value;
                }

                return string.Empty;
            }
        }

        public string UserEmail
        {
            get
            {
                if (this.ClaimsPrincipal != null)
                {
                    return this.ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                }

                return string.Empty;
            }
        }

        public string SessionId
        {
            get
            {
                if (this.ClaimsPrincipal != null)
                {
                    return this.ClaimsPrincipal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId)?.Value;
                }

                return string.Empty;
            }
        }

        private ClaimsPrincipal ClaimsPrincipal => this.httpContextAccessor.HttpContext?.User;
    }
}
