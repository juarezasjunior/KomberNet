// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Application.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Domain.Entities;
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Infrastructure.DatabaseRepositories.DatabaseEntities;
    using MediatR;

    public partial class CountryQueryHandler : IRequestHandler<CountryQueryRequest, CountryQueryResponse>
    {
        private readonly IApplicationDatabaseRepository applicationDatabaseRepository;

        public CountryQueryHandler(IApplicationDatabaseRepository applicationDatabaseRepository)
        {
            this.applicationDatabaseRepository = applicationDatabaseRepository;
        }

        public async Task<CountryQueryResponse> Handle(CountryQueryRequest request, CancellationToken cancellationToken)
        {
            var result = (await this.applicationDatabaseRepository.GetByConditionAsync<TbCountry, Country>(
                x => x.Where(y => y.CountryId == request.CountryId), cancellationToken))?.FirstOrDefault();

            return new CountryQueryResponse() { Entity = result };
        }
    }
}
