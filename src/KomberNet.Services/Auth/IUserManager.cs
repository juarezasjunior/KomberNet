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
        public Task<IdentityResult> CreateAsync(TbUser user, string password);

        public Task<IdentityResult> UpdateAsync(TbUser user);

        public Task<IdentityResult> DeleteAsync(TbUser user);

        public Task<TbUser> FindByIdAsync(string userId);

        public Task<TbUser> FindByNameAsync(string userName);

        public Task<TbUser> FindByEmailAsync(string email);

        public Task<bool> CheckPasswordAsync(TbUser user, string password);

        public Task<IList<Claim>> GetClaimsAsync(TbUser user);

        public Task<IdentityResult> AddClaimAsync(TbUser user, Claim claim);

        public Task<IdentityResult> RemoveClaimAsync(TbUser user, Claim claim);

        public Task<IdentityResult> AddToRoleAsync(TbUser user, string roleName);

        public Task<IdentityResult> RemoveFromRoleAsync(TbUser user, string roleName);

        public Task<IList<string>> GetRolesAsync(TbUser user);

        public Task<bool> IsInRoleAsync(TbUser user, string roleName);

        public Task<IList<TbUser>> GetUsersInRoleAsync(string roleName);

        public Task<IdentityResult> AddLoginAsync(TbUser user, UserLoginInfo login);

        public Task<IdentityResult> RemoveLoginAsync(TbUser user, string loginProvider, string providerKey);

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TbUser user);

        public Task<bool> HasPasswordAsync(TbUser user);

        public Task<IdentityResult> AddPasswordAsync(TbUser user, string password);

        public Task<IdentityResult> ChangePasswordAsync(TbUser user, string currentPassword, string newPassword);

        public Task<IdentityResult> RemovePasswordAsync(TbUser user);

        public Task<IdentityResult> ConfirmEmailAsync(TbUser user, string token);

        public Task<bool> IsEmailConfirmedAsync(TbUser user);

        public Task<string> GenerateEmailConfirmationTokenAsync(TbUser user);

        public Task<string> GeneratePasswordResetTokenAsync(TbUser user);

        public Task<IdentityResult> ResetPasswordAsync(TbUser user, string token, string newPassword);

        public Task<bool> IsPhoneNumberConfirmedAsync(TbUser user);

        public Task<IdentityResult> SetPhoneNumberAsync(TbUser user, string phoneNumber);

        public Task<string> GetPhoneNumberAsync(TbUser user);

        public Task<IdentityResult> ChangePhoneNumberAsync(TbUser user, string phoneNumber, string token);

        public Task<IdentityResult> SetTwoFactorEnabledAsync(TbUser user, bool enabled);
    }
}
