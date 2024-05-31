// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ApplicationUserManager : UserManager<TbUser>, IUserManager
    {
        public ApplicationUserManager(IUserStore<TbUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TbUser> passwordHasher, IEnumerable<IUserValidator<TbUser>> userValidators, IEnumerable<IPasswordValidator<TbUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TbUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
