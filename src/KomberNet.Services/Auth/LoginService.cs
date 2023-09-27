﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Identity;

    public class LoginService : ILoginService
    {
        private readonly UserManager<TbUser> userManager;
        private readonly ITokenService tokenService;

        public LoginService(
            UserManager<TbUser> userManager,
            ITokenService tokenService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await this.ValidateAsync(request, cancellationToken);

            var result = await this.tokenService.GenerateTokenAsync(user, cancellationToken);

            return new LoginResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken,
            };
        }

        private async Task<TbUser> ValidateAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await this.userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new KomberNetSecurityException();
            }

            var isPasswordValid = await this.userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                throw new KomberNetSecurityException();
            }

            return user;
        }
    }
}