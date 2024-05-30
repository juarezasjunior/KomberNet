// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Organization
{
    using KomberNet.Contracts;
    using KomberNet.Models.Organization;

    public interface IOrganizationGroupService : ITransientService
    {
        Task<OrganizationGroupQueryResponse> GetAsync(OrganizationGroupQueryRequest request);

        Task<OrganizationGroupsQueryResponse> GetAsync(OrganizationGroupsQueryRequest request);

        Task<OrganizationGroupHandlerResponse> HandleAsync(OrganizationGroupHandlerRequest request);
    }
}
