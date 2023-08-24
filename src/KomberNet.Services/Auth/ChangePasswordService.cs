// Copyright Contributors to the KomberNet project.
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

    public class ChangePasswordService : IChangePasswordService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<TbApplicationUser> userManager;

        public ChangePasswordService(
            ICurrentUserService currentUserService,
            UserManager<TbApplicationUser> userManager)
        {
            this.currentUserService = currentUserService;
            this.userManager = userManager;
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentUserId = this.currentUserService.CurrentUserId;
            var applicationUser = await this.userManager.FindByIdAsync(currentUserId.ToString());

            if (applicationUser == null)
            {
                throw new SecurityException();
            }

            var result = await this.userManager.ChangePasswordAsync(
                applicationUser,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new KomberNetException(exceptionCode: ExceptionCode.InvalidPassword, additionalInfo: errors);
            }

            return new ChangePasswordResponse();
        }
    }
}
