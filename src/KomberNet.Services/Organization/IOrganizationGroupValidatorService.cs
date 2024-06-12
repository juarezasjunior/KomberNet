// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Organization
{
    using KomberNet.Contracts;
    using KomberNet.Models.Organization;

    public interface IOrganizationGroupValidatorService : ITransientService
    {
        public Task ValidateGetAsync(OrganizationGroupGetRequest request, CancellationToken cancellationToken = default);

        public Task ValidateHandlerAsync(OrganizationGroupHandlerRequest request, CancellationToken cancellationToken = default);
    }
}
