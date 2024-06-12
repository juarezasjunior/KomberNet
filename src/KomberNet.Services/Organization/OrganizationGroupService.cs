// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Organization
{
    using KomberNet.Contracts;
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Organization;
    using KomberNet.Models.Contracts;
    using KomberNet.Models.Organization;

    public class OrganizationGroupService : BaseService, IOrganizationGroupService
    {
        private readonly IDatabaseRepository databaseRepository;
        private readonly IOrganizationGroupValidatorService organizationGroupValidatorService;
        private readonly ICurrentUserService currentUserService;

        public OrganizationGroupService(
            IDatabaseRepository databaseRepository,
            IOrganizationGroupValidatorService organizationGroupValidatorService,
            ICurrentUserService currentUserService)
        {
            this.databaseRepository = databaseRepository;
            this.organizationGroupValidatorService = organizationGroupValidatorService;
            this.currentUserService = currentUserService;
        }

        public async Task<OrganizationGroupGetResponse> GetAsync(OrganizationGroupGetRequest request, CancellationToken cancellationToken = default)
        {
            await this.organizationGroupValidatorService.ValidateGetAsync(request, cancellationToken);

            return new OrganizationGroupGetResponse()
            {
                Entity = (await this.databaseRepository.GetByConditionAsync<TbOrganizationGroup, OrganizationGroup>(
                    x => x.Where(y => y.OrgazationGroupId == request.OrganizationGroupId),
                    cancellationToken)).FirstOrDefault(),
            };
        }

        public async Task<OrganizationGroupHandlerResponse> HandleAsync(OrganizationGroupHandlerRequest request, CancellationToken cancellationToken = default)
        {
            await this.organizationGroupValidatorService.ValidateHandlerAsync(request, cancellationToken);

            var tbOrganizationGroup = this.databaseRepository.ApplyChanges<TbOrganizationGroup, OrganizationGroup>(request.Entity);

            if (request.Entity.DataState == DataState.Created)
            {
                tbOrganizationGroup.OrganizationGroupUsers.Add(
                    new TbOrganizationGroupUser()
                    {
                        UserId = this.currentUserService.UserId,
                        IsAdmin = true,
                    });
            }

            await this.databaseRepository.SaveAsync(cancellationToken);

            return new OrganizationGroupHandlerResponse()
            {
                OrganizationGroupId = tbOrganizationGroup.OrgazationGroupId,
            };
        }
    }
}
