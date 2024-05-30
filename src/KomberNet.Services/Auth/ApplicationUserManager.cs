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

    public class ApplicationUserManager : UserManager<SysUser>, IUserManager
    {
        public ApplicationUserManager(IUserStore<SysUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<SysUser> passwordHasher, IEnumerable<IUserValidator<SysUser>> userValidators, IEnumerable<IPasswordValidator<SysUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<SysUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
