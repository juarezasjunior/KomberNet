// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Backend.Tests.Auth
{
    using System.Threading.Tasks;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using KomberNet.Exceptions;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using KomberNet.Services.Auth;
    using KomberNet.Tests;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class LoginTests : BaseTest
    {
        [Test]
        [Description(@"Given an user
                       When I try to login with an invalid email
                       Then I am prevented to login")]
        public async Task ShouldPreventInvalidEmail()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager<TbUser>>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            await loginService.LoginAsync(loginRequest, CancellationToken.None)
                .ShouldThrowAsync(typeof(KomberNetSecurityException));

            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When I try to login with an invalid password
                       Then I am prevented to login")]
        public async Task ShouldPreventInvalidPassword()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager<TbUser>>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<TbUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => false)
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            await loginService.LoginAsync(loginRequest, CancellationToken.None)
                .ShouldThrowAsync(typeof(KomberNetSecurityException));

            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When I try to login
                       Then I can login")]
        public async Task ShouldLogin()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var loginRequest = fixture.Create<LoginRequest>();

            var userManagerMock = fixture.Freeze<Mock<IUserManager<TbUser>>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<TbUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => true)
                .Verifiable();

            var tokenServiceMock = fixture.Freeze<Mock<ITokenService>>();
            tokenServiceMock.Setup(x => x.GenerateTokenAsync(It.IsAny<TbUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => (fixture.Create<string>(), fixture.Create<string>()))
                .Verifiable();

            var loginService = fixture.Create<LoginService>();

            var loginResponse = await loginService.LoginAsync(loginRequest, CancellationToken.None);

            loginResponse.Token.ShouldNotBeNull();
            loginResponse.RefreshToken.ShouldNotBeNull();
            userManagerMock.VerifyAll();
            tokenServiceMock.VerifyAll();
        }
    }
}
