// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Organization
{
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Models.Organization;

    public class OrganizationGroupService : IOrganizationGroupService
    {
public OrganizationGroupService(IDatabaseRepository databaseRepository)
{
    
}

        public Task<OrganizationGroupQueryResponse> GetAsync(OrganizationGroupQueryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrganizationGroupsQueryResponse> GetAsync(OrganizationGroupsQueryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrganizationGroupHandlerResponse> HandleAsync(OrganizationGroupHandlerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
