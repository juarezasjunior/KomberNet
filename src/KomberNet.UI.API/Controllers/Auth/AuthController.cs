// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Controllers.Auth
{
    using System;
    using System.Threading.Tasks;
    using KomberNet.Models.Auth;
    using KomberNet.Services;
    using KomberNet.Services.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/api/[controller]/[action]")]

    public partial class AuthController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly ILogoutService logoutService;
        private readonly IRefreshTokenService refreshTokenService;
        private readonly IUserHandlerService userHandlerService;
        private readonly IChangePasswordService changePasswordService;

        public AuthController(
            ILoginService loginService,
            ILogoutService logoutService,
            IRefreshTokenService refreshTokenService,
            IUserHandlerService userHandlerService,
            IChangePasswordService changePasswordService)
        {
            this.loginService = loginService;
            this.logoutService = logoutService;
            this.refreshTokenService = refreshTokenService;
            this.userHandlerService = userHandlerService;
            this.changePasswordService = changePasswordService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertUserAsync([FromBody] UserInsertRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.userHandlerService.InsertUserAsync(request, cancellationToken));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.loginService.LoginAsync(request, cancellationToken));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.refreshTokenService.RefreshTokenAsync(request, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.logoutService.LogoutAsync(request, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogoutAllSessionsAsync([FromBody] LogoutAllSessionsRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.logoutService.LogoutAllSessionsAsync(request, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            return this.Ok(await this.changePasswordService.ChangePasswordAsync(request, cancellationToken));
        }
    }
}