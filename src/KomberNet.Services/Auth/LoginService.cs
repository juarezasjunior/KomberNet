// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using Microsoft.Extensions.Caching.Distributed;

    public class LoginService : BaseService, ILoginService
    {
        private readonly IUserManager userManager;
        private readonly IDistributedCache distributedCache;
        private readonly ITokenService tokenService;

        public LoginService(
            IUserManager userManager,
            IDistributedCache distributedCache,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.distributedCache = distributedCache;
            this.tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await this.ValidateAsync(request, cancellationToken);

            await this.distributedCache.RemoveAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, user.Email));

            var result = await this.tokenService.GenerateTokenAsync(user, cancellationToken);

            return new LoginResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
            };
        }

        private async Task<SysUser> ValidateAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await this.userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new KomberNetException(ExceptionCode.Auth_User_InvalidLogin);
            }

            var isPasswordValid = await this.userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                throw new KomberNetException(ExceptionCode.Auth_User_InvalidLogin);
            }

            return user;
        }
    }
}
