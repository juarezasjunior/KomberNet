// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Services.Auth
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;

    public interface IUserManager<TUser>
        where TUser : class
    {
        public Task<IdentityResult> CreateAsync(TUser user, string password);

        public Task<IdentityResult> UpdateAsync(TUser user);

        public Task<IdentityResult> DeleteAsync(TUser user);

        public Task<TUser> FindByIdAsync(string userId);

        public Task<TUser> FindByNameAsync(string userName);

        public Task<TUser> FindByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(TUser user, string password);

        public Task<IList<Claim>> GetClaimsAsync(TUser user);

        public Task<IdentityResult> AddClaimAsync(TUser user, Claim claim);

        public Task<IdentityResult> RemoveClaimAsync(TUser user, Claim claim);

        public Task<IdentityResult> AddToRoleAsync(TUser user, string roleName);

        public Task<IdentityResult> RemoveFromRoleAsync(TUser user, string roleName);

        public Task<IList<string>> GetRolesAsync(TUser user);

        public Task<bool> IsInRoleAsync(TUser user, string roleName);

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName);

        public Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login);

        public Task<IdentityResult> RemoveLoginAsync(TUser user, string loginProvider, string providerKey);

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);

        public Task<bool> HasPasswordAsync(TUser user);

        public Task<IdentityResult> AddPasswordAsync(TUser user, string password);

        public Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword);

        public Task<IdentityResult> RemovePasswordAsync(TUser user);

        public Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);

        public Task<bool> IsEmailConfirmedAsync(TUser user);

        public Task<string> GenerateEmailConfirmationTokenAsync(TUser user);

        public Task<string> GeneratePasswordResetTokenAsync(TUser user);

        public Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);

        public Task<bool> IsPhoneNumberConfirmedAsync(TUser user);

        public Task<IdentityResult> SetPhoneNumberAsync(TUser user, string phoneNumber);

        public Task<string> GetPhoneNumberAsync(TUser user);

        public Task<IdentityResult> ChangePhoneNumberAsync(TUser user, string phoneNumber, string token);

        public Task<IdentityResult> SetTwoFactorEnabledAsync(TUser user, bool enabled);
    }
}
