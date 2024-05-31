// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Organization
{
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Organization;
    using KomberNet.Models.Organization;

    public class OrganizationGroupService : IOrganizationGroupService
    {
        private readonly IDatabaseRepository databaseRepository;

        public OrganizationGroupService(IDatabaseRepository databaseRepository)
        {
            this.databaseRepository = databaseRepository;
        }

        public async Task<OrganizationGroupQueryResponse> GetAsync(OrganizationGroupQueryRequest request)
        {
            return new OrganizationGroupQueryResponse()
            {
                Entity = (await this.databaseRepository.GetByConditionAsync<TbOrganizationGroup, OrganizationGroup>(x =>
                    x.Where(y => y.OrgazationGroupId == request.OrganizationGroupId))).FirstOrDefault(),
            };
        }

        public Task<OrganizationGroupHandlerResponse> HandleAsync(OrganizationGroupHandlerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
