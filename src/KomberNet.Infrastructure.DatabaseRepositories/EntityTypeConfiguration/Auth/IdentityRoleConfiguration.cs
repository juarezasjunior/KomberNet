// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Infrastructure.DatabaseRepositories.EntityTypeConfiguration.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            foreach (var identityRole in GetPermissions())
            {
                builder.HasData(identityRole);
            }
        }

        private static IEnumerable<IdentityRole<Guid>> GetPermissions()
        {
            yield return new IdentityRole<Guid>()
            {
                Id = Guid.Parse("75F91C5C-5CC9-415E-9CBE-A5BE0C9A7A67"),
                Name = nameof(APIRoles.Administrator),
                NormalizedName = nameof(APIRoles.Administrator),
            };

            yield return new IdentityRole<Guid>()
            {
                Id = Guid.Parse("C332308B-DB6D-4F86-9AAB-7036368B78A0"),
                Name = nameof(APIRoles.Integration),
                NormalizedName = nameof(APIRoles.Integration),
            };

            yield return new IdentityRole<Guid>()
            {
                Id = Guid.Parse("6E7D45E2-1D90-4C14-BCBD-A17821555EFF"),
                Name = nameof(APIRoles.Support),
                NormalizedName = nameof(APIRoles.Support),
            };

            yield return new IdentityRole<Guid>()
            {
                Id = Guid.Parse("63906779-974F-4FCE-B91A-28571D762F56"),
                Name = nameof(APIRoles.User),
                NormalizedName = nameof(APIRoles.User),
            };
        }
    }
}
