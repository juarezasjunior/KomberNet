// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.ActionFilters
{
    using System.Linq;
    using System.Threading.Tasks;
    using KomberNet.Contracts;
    using KomberNet.Services.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AuthorizationActionFilter : IAsyncAuthorizationFilter
    {
        private readonly ICurrentUserValidatorService currentUserValidatorService;

        public AuthorizationActionFilter(
            ICurrentUserValidatorService currentUserValidatorService)
        {
            this.currentUserValidatorService = currentUserValidatorService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute)))
            {
                await this.currentUserValidatorService.ValidateAsync(CancellationToken.None);
            }
        }
    }
}
