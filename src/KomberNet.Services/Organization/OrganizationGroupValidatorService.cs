// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Organization
{
    using KomberNet.Models.Organization;

    public class OrganizationGroupValidatorService : BaseService, IOrganizationGroupValidatorService
    {
        public Task ValidateGetAsync(OrganizationGroupGetRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ValidateHandlerAsync(OrganizationGroupHandlerRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
