// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Backend.Tests.Auth
{
    using System.Threading.Tasks;
    using AutoFixture;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using KomberNet.Services.Auth;
    using KomberNet.Tests;
    using KomberNet.Tests.Extensions;
    using Microsoft.Extensions.Caching.Distributed;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class LoginTests : BaseTest
    {
        [Test]
        [Description(@"Given an user
                       When he tries to login with an invalid email
                       Then he is prevented to login")]
        public async Task ShouldPreventInvalidEmail()
        {
            var fixture = this.GetNewFixture();

            var sysUser = fixture.Create<SysUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            await loginService.LoginAsync(loginRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.Auth_User_InvalidLogin);

            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to login with an invalid password
                       Then he is prevented to login")]
        public async Task ShouldPreventInvalidPassword()
        {
            var fixture = this.GetNewFixture();

            var sysUser = fixture.Create<SysUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => sysUser)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<SysUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => false)
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            await loginService.LoginAsync(loginRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.Auth_User_InvalidLogin);

            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to login
                       Then he can login")]
        public async Task ShouldLogin()
        {
            var fixture = this.GetNewFixture();

            var sysUser = fixture.Create<SysUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => sysUser)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<SysUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => true)
                .Verifiable();

            var tokenServiceMock = fixture.Freeze<Mock<ITokenService>>();
            tokenServiceMock.Setup(x => x.GenerateTokenAsync(It.IsAny<SysUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => (fixture.Create<string>(), fixture.Create<string>()))
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            var loginResponse = await loginService.LoginAsync(loginRequest, CancellationToken.None);

            loginResponse.Token.ShouldNotBeNull();
            loginResponse.RefreshToken.ShouldNotBeNull();
            userManagerMock.VerifyAll();
            tokenServiceMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user that has loggout all sessions before
                       When he tries to login
                       Then he can login")]
        public async Task ShouldLoginWhenHasLoggoutAllSessionsBefore()
        {
            var fixture = this.GetNewFixture();

            var sysUser = fixture.Create<SysUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => sysUser)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<SysUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => true)
                .Verifiable();

            var tokenServiceMock = fixture.Freeze<Mock<ITokenService>>();
            tokenServiceMock.Setup(x => x.GenerateTokenAsync(It.IsAny<SysUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => (fixture.Create<string>(), fixture.Create<string>()))
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.RemoveAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, sysUser.Email), It.IsAny<CancellationToken>()))
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            var loginResponse = await loginService.LoginAsync(loginRequest, CancellationToken.None);

            loginResponse.Token.ShouldNotBeNull();
            loginResponse.RefreshToken.ShouldNotBeNull();
            userManagerMock.VerifyAll();
            tokenServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
        }
    }
}
