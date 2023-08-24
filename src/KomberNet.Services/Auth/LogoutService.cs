// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;

    public class LogoutService : ILogoutService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ILogoutEmailService logoutEmailService;

        public LogoutService(
            ICurrentUserService currentUserService,
            ILogoutEmailService logoutEmailService)
        {
            this.currentUserService = currentUserService;
            this.logoutEmailService = logoutEmailService;
        }

        public async Task<LogoutResponse> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await this.logoutEmailService.LogoutEmailAsync(this.currentUserService.CurrentUserEmail, cancellationToken);

            return new LogoutResponse();
        }
    }
}
