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
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ChangePasswordTests : BaseTest
    {
        [Test]
        [Description(@"Given an user that was deleted
                       When he tries to change its password
                       Then he cannot change")]
        public async Task ShouldNotChangePasswordWhenAnUserIsNotFound()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var changePasswordRequest = fixture.Create<ChangePasswordRequest>();

            var currentUserServiceMock = fixture.Freeze<Mock<ICurrentUserService>>();
            currentUserServiceMock.Setup(x => x.CurrentUserId).Returns(tbUser.Id);

            var userManagerMock = fixture.Freeze<Mock<IUserManager<TbUser>>>();
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var changePasswordService = fixture.Create<ChangePasswordService>();

            await changePasswordService.ChangePasswordAsync(changePasswordRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to change its password
                       But its old password or its new password is invalid
                       Then he cannot change")]
        public async Task ShouldNotChangePasswordWhenPasswordIsInvalid()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var changePasswordRequest = fixture.Create<ChangePasswordRequest>();

            var currentUserServiceMock = fixture.Freeze<Mock<ICurrentUserService>>();
            currentUserServiceMock.Setup(x => x.CurrentUserId).Returns(tbUser.Id);

            var userManagerMock = fixture.Freeze<Mock<IUserManager<TbUser>>>();
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();

            var identityResult = IdentityResult.Failed(new IdentityError() { Code = fixture.Create<string>(), Description = fixture.Create<string>() });

            userManagerMock.Setup(x => x.ChangePasswordAsync(tbUser, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword))
                .ReturnsAsync(() => identityResult)
                .Verifiable();

            var changePasswordService = fixture.Create<ChangePasswordService>();

            await changePasswordService.ChangePasswordAsync(changePasswordRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.Auth_User_InvalidPassword);

            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to change its password
                       Then he can change it")]
        public async Task ShouldChangePassword()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var changePasswordRequest = fixture.Create<ChangePasswordRequest>();

            var currentUserServiceMock = fixture.Freeze<Mock<ICurrentUserService>>();
            currentUserServiceMock.Setup(x => x.CurrentUserId).Returns(tbUser.Id);

            var userManagerMock = fixture.Freeze<Mock<IUserManager<TbUser>>>();
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();

            userManagerMock.Setup(x => x.ChangePasswordAsync(tbUser, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword))
                .ReturnsAsync(() => IdentityResult.Success)
                .Verifiable();

            var changePasswordService = fixture.Create<ChangePasswordService>();

            var result = await changePasswordService.ChangePasswordAsync(changePasswordRequest, CancellationToken.None);

            result.ShouldNotBeNull();

            userManagerMock.VerifyAll();
        }
    }
}
