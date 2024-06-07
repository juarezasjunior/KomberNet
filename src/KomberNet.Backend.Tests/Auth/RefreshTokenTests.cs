// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Backend.Tests.Auth
{
    using System.Security.Claims;
    using System.Text;
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
    public class RefreshTokenTests : BaseTest
    {
        [Test]
        [Description(@"Given an user that has loggout all sessions before
                       When he tries to refresh a token
                       Then he cannot refresh")]
        public async Task ShouldPreventRefreshTokenWhenUserHasLoggoutAllSessions()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var refreshTokenRequest = fixture.Create<RefreshTokenRequest>();

            var claimsPrincipalServiceMock = fixture.Freeze<Mock<IClaimsPrincipalService>>();
            claimsPrincipalServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(() => GetClaimsPrincipal(tbUser))
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, tbUser.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes("true"))
                .Verifiable();

            var refreshTokenService = fixture.Create<RefreshTokenService>();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to refresh a token with a non-existing email
                       Then he cannot refresh")]
        public async Task ShouldPreventRefreshTokenWithANonExistingEmail()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var refreshTokenRequest = fixture.Create<RefreshTokenRequest>();

            var claimsPrincipalServiceMock = fixture.Freeze<Mock<IClaimsPrincipalService>>();
            claimsPrincipalServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(() => GetClaimsPrincipal(tbUser))
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, tbUser.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var refreshTokenService = fixture.Create<RefreshTokenService>();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to refresh a non-existing refresh token
                       Then he cannot refresh")]
        public async Task ShouldPreventRefreshANonExistingRefreshToken()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var refreshTokenRequest = fixture.Create<RefreshTokenRequest>();
            var principal = GetClaimsPrincipal(tbUser);
            var sessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId).Value;

            var claimsPrincipalServiceMock = fixture.Freeze<Mock<IClaimsPrincipalService>>();
            claimsPrincipalServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(() => principal)
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, tbUser.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();

            var refreshTokenService = fixture.Create<RefreshTokenService>();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to refresh an expired token
                       Then he cannot refresh")]
        public async Task ShouldPreventRefreshAnExpiredToken()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var refreshTokenRequest = fixture.Create<RefreshTokenRequest>();
            var principal = GetClaimsPrincipal(tbUser);
            var sessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId).Value;

            var claimsPrincipalServiceMock = fixture.Freeze<Mock<IClaimsPrincipalService>>();
            claimsPrincipalServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(() => principal)
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, tbUser.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes(refreshTokenRequest.RefreshToken))
                .Verifiable();

            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();

            var refreshTokenService = fixture.Create<RefreshTokenService>();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();

            // Should prevent when the stored date is invalid
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes("invalid stored date"))
                .Verifiable();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();

            // Should prevent when the stored date is less than the actual date/time
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes(DateTime.Now.AddHours(-1).ToString()))
                .Verifiable();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to refresh a token from another user
                       Then he cannot refresh")]
        public async Task ShouldPreventRefreshATokenFromAnotherUser()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var refreshTokenRequest = fixture.Create<RefreshTokenRequest>();
            var principal = GetClaimsPrincipal(tbUser);
            var sessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId).Value;

            var claimsPrincipalServiceMock = fixture.Freeze<Mock<IClaimsPrincipalService>>();
            claimsPrincipalServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(() => principal)
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, tbUser.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes(fixture.Create<string>()))
                .Verifiable();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();

            var refreshTokenService = fixture.Create<RefreshTokenService>();

            await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None)
                .ShouldThrowKomberNetExceptionAsync(ExceptionCode.SecurityValidation);

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();
        }

        [Test]
        [Description(@"Given an user
                       When he tries to refresh a token
                       Then he can refresh")]
        public async Task ShouldRefreshAToken()
        {
            var fixture = this.GetNewFixture();

            var tbUser = fixture.Create<TbUser>();
            var refreshTokenRequest = fixture.Create<RefreshTokenRequest>();
            var principal = GetClaimsPrincipal(tbUser);
            var sessionId = principal.Claims.FirstOrDefault(x => x.Type == KomberNetClaims.SessionId).Value;

            var claimsPrincipalServiceMock = fixture.Freeze<Mock<IClaimsPrincipalService>>();
            claimsPrincipalServiceMock.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(() => principal)
                .Verifiable();

            var distributedCacheMock = fixture.Freeze<Mock<IDistributedCache>>();
            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.UserHasLogoutAllSessionsKey, tbUser.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null)
                .Verifiable();

            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes(refreshTokenRequest.RefreshToken))
                .Verifiable();

            distributedCacheMock.Setup(x => x.GetAsync(string.Format(JwtCacheKeys.RefreshTokenExpirationTimeKey, tbUser.Email, sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes(DateTime.Now.AddHours(1).ToString()))
                .Verifiable();

            var userManagerMock = fixture.Freeze<Mock<IUserManager>>();
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => tbUser)
                .Verifiable();

            var tokenServiceMock = fixture.Freeze<Mock<ITokenService>>();
            tokenServiceMock.Setup(x => x.GenerateTokenAsync(tbUser, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => (refreshTokenRequest.Token, fixture.Create<string>()))
                .Verifiable();

            var refreshTokenService = fixture.Create<RefreshTokenService>();

            var refreshedToken = await refreshTokenService.RefreshTokenAsync(refreshTokenRequest, CancellationToken.None);

            refreshedToken.Token.ShouldNotBeNull();
            refreshedToken.RefreshToken.ShouldNotBeNull();

            claimsPrincipalServiceMock.VerifyAll();
            distributedCacheMock.VerifyAll();
            userManagerMock.VerifyAll();
        }

        private static ClaimsPrincipal GetClaimsPrincipal(TbUser tbUser)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, tbUser.Email),
                new Claim(KomberNetClaims.SessionId, Guid.NewGuid().ToString()),
            };

            return new ClaimsPrincipal(new List<ClaimsIdentity>() { new ClaimsIdentity(claims) });
        }
    }
}
