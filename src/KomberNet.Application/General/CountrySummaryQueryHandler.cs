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

    public partial class CountrySummaryQueryHandler : IRequestHandler<CountrySummaryQueryRequest, CountrySummaryQueryResponse>
    {
        private readonly IApplicationDatabaseRepository applicationDatabaseRepository;

        public CountrySummaryQueryHandler(IApplicationDatabaseRepository applicationDatabaseRepository)
        {
            this.applicationDatabaseRepository = applicationDatabaseRepository;
        }

        public async Task<CountrySummaryQueryResponse> Handle(CountrySummaryQueryRequest request, CancellationToken cancellationToken)
        {
            var result = (await this.applicationDatabaseRepository.GetByConditionAsync<TbCountry, CountrySummary>(
                x => x.Where(y => y.CountryId == request.CountryId), cancellationToken))?.FirstOrDefault();

            return new CountrySummaryQueryResponse() { Summary = result };
        }
    }
}
