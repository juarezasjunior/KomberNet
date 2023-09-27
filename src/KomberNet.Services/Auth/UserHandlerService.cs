﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Identity;

    public class UserHandlerService : IUserHandlerService
    {
        private readonly UserManager<TbUser> userManager;

        public UserHandlerService(UserManager<TbUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<UserInsertResponse> InsertUserAsync(UserInsertRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new TbUser()
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await this.userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new KomberNetException(exceptionCode: ExceptionCode.InvalidPassword, additionalInfo: errors);
            }

            await this.userManager.AddToRoleAsync(user, nameof(APIRoles.User));

            return new UserInsertResponse();
        }
    }
}