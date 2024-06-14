// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Controllers.Organization
{
    using System;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using KomberNet.Models.Organization;
    using KomberNet.Services;
    using KomberNet.Services.Auth;
    using KomberNet.Services.Organization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/Organization/[controller]/[action]")]

    public partial class OrganizationGroupController : ControllerBase
    {
        private readonly IOrganizationGroupService organizationGroupService;

        public OrganizationGroupController(
            IOrganizationGroupService organizationGroupService)
        {
            this.organizationGroupService = organizationGroupService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromQuery] OrganizationGroupGetRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.organizationGroupService.GetAsync(request, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] OrganizationGroupHandlerRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.organizationGroupService.HandleAsync(request, cancellationToken));
        }
    }
}