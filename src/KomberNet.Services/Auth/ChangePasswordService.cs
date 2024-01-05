// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Linq;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;

    public class ChangePasswordService : BaseService, IChangePasswordService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUserManager<TbUser> userManager;

        public ChangePasswordService(
            ICurrentUserService currentUserService,
            IUserManager<TbUser> userManager)
        {
            this.currentUserService = currentUserService;
            this.userManager = userManager;
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentUserId = this.currentUserService.CurrentUserId;
            var user = await this.userManager.FindByIdAsync(currentUserId.ToString());

            if (user == null)
            {
                throw new KomberNetSecurityException();
            }

            var result = await this.userManager.ChangePasswordAsync(
                user,
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
