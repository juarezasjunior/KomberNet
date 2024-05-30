// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Backend.Tests.Auth
{
    using System.Text;
    using System.Threading.Tasks;
    using AutoFixture;
    using KomberNet.Contracts;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using KomberNet.Services.Auth;
    using KomberNet.Tests;
    using Microsoft.Extensions.Caching.Distributed;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class LogoutTests : BaseTest
    {
        [Test]
        [Description(@"Given a logged user
                       When he tries to logout
                       Then he can logout
                        And his refresh token is expired")]
        public async Task ShouldLogout()
        {
            var fixture = this.GetNewFixture();

            var sysUser = fixture.Create<SysUser>();
            var sessionId = fixture.Create<string>();
            var logoutRequest = fixture.Create<LogoutRequest>();

            var currentUserServiceMock = fixture.Freeze<Mock<ICurrentUserService>>();
            currentUserServiceMock.Setup(x => x.UserEmail).Returns(sysUser.Email);
            currentUserServiceMock.Setup(x => x.SessionId).Returns(sessionId);

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x =>
                x.SetAsync(
                    string.Format(JwtCacheKeys.UserHasLogoutKey, sysUser.Email, sessionId),
                    Encoding.UTF8.GetBytes("true"),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Verifiable();

            distributedCacheMock.Setup(x => x.RemoveAsync(string.Format(JwtCacheKeys.RefreshTokenKey, sysUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .Verifiable();

            distributedCacheMock.Setup(x => x.RemoveAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, sysUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .Verifiable();

            var logoutService = fixture.Create<LogoutService>();

            var result = await logoutService.LogoutAsync(logoutRequest, CancellationToken.None);

            result.ShouldNotBeNull();

            distributedCacheMock.VerifyAll();
        }

        [Test]
        [Description(@"Given a logged user
                       When he tries to logout all sessions
                       Then he can logout all sessions")]
        public async Task ShouldLogoutAllSessions()
        {
            var fixture = this.GetNewFixture();

            var sysUser = fixture.Create<SysUser>();
            var logoutAllSessionsRequest = fixture.Create<LogoutAllSessionsRequest>();

            var currentUserServiceMock = fixture.Freeze<Mock<ICurrentUserService>>();
            currentUserServiceMock.Setup(x => x.UserEmail).Returns(sysUser.Email);

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();

            distributedCacheMock.Setup(x =>
                x.SetAsync(
                    string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, sysUser.Email),
                    Encoding.UTF8.GetBytes("true"),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Verifiable();

            var logoutService = fixture.Create<LogoutService>();

            var result = await logoutService.LogoutAllSessionsAsync(logoutAllSessionsRequest, CancellationToken.None);

            result.ShouldNotBeNull();

            distributedCacheMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an email of an user
                       When I try to logout all sessions of this email
                       Then I can logout all sessions")]
        public async Task ShouldLogoutAllSessionsOfAnEmail()
        {
            var fixture = this.GetNewFixture();

            var email = fixture.Create<string>();
            var logoutAllSessionsRequest = fixture.Create<LogoutAllSessionsRequest>();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();

            distributedCacheMock.Setup(x =>
                x.SetAsync(
                    string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, email),
                    Encoding.UTF8.GetBytes("true"),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Verifiable();

            var logoutService = fixture.Create<LogoutService>();

            await logoutService.LogoutAllSessionsAsync(email, CancellationToken.None);

            distributedCacheMock.VerifyAll();
        }
    }
}
