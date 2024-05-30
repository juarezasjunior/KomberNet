// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using KomberNet.Contracts;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using Microsoft.AspNetCore.Identity;

    public interface IUserManager : ITransientService
    {
        public Task<IdentityResult> CreateAsync(SysUser user, string password);

        public Task<IdentityResult> UpdateAsync(SysUser user);

        public Task<IdentityResult> DeleteAsync(SysUser user);

        public Task<SysUser> FindByIdAsync(string userId);

        public Task<SysUser> FindByNameAsync(string userName);

        public Task<SysUser> FindByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(SysUser user, string password);

        public Task<IList<Claim>> GetClaimsAsync(SysUser user);

        public Task<IdentityResult> AddClaimAsync(SysUser user, Claim claim);

        public Task<IdentityResult> RemoveClaimAsync(SysUser user, Claim claim);

        public Task<IdentityResult> AddToRoleAsync(SysUser user, string roleName);

        public Task<IdentityResult> RemoveFromRoleAsync(SysUser user, string roleName);

        public Task<IList<string>> GetRolesAsync(SysUser user);

        public Task<bool> IsInRoleAsync(SysUser user, string roleName);

        public Task<IList<SysUser>> GetUsersInRoleAsync(string roleName);

        public Task<IdentityResult> AddLoginAsync(SysUser user, UserLoginInfo login);

        public Task<IdentityResult> RemoveLoginAsync(SysUser user, string loginProvider, string providerKey);

        public Task<IList<UserLoginInfo>> GetLoginsAsync(SysUser user);

        public Task<bool> HasPasswordAsync(SysUser user);

        public Task<IdentityResult> AddPasswordAsync(SysUser user, string password);

        public Task<IdentityResult> ChangePasswordAsync(SysUser user, string currentPassword, string newPassword);

        public Task<IdentityResult> RemovePasswordAsync(SysUser user);

        public Task<IdentityResult> ConfirmEmailAsync(SysUser user, string token);

        public Task<bool> IsEmailConfirmedAsync(SysUser user);

        public Task<string> GenerateEmailConfirmationTokenAsync(SysUser user);

        public Task<string> GeneratePasswordResetTokenAsync(SysUser user);

        public Task<IdentityResult> ResetPasswordAsync(SysUser user, string token, string newPassword);

        public Task<bool> IsPhoneNumberConfirmedAsync(SysUser user);

        public Task<IdentityResult> SetPhoneNumberAsync(SysUser user, string phoneNumber);

        public Task<string> GetPhoneNumberAsync(SysUser user);

        public Task<IdentityResult> ChangePhoneNumberAsync(SysUser user, string phoneNumber, string token);

        public Task<IdentityResult> SetTwoFactorEnabledAsync(SysUser user, bool enabled);
    }
}
