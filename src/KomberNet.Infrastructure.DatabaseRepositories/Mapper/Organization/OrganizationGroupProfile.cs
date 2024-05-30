// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Infrastructure.DatabaseRepositories.Organization
{
    using AutoMapper;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Organization;
    using KomberNet.Models.Organization;

    public class OrganizationGroupProfile : Profile
    {
        public OrganizationGroupProfile()
        {
            this.CreateMap<OrganizationGroup, SysOrganizationGroup>().ReverseMap();
        }
    }
}
