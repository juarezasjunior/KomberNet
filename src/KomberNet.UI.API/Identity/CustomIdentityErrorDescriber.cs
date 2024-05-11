// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Identity
{
    using KomberNet.Resources;
    using Microsoft.AspNetCore.Identity;

    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateRoleName(string role)
        {
            var identityError = base.DuplicateRoleName(role);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, role);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError InvalidRoleName(string role)
        {
            var identityError = base.InvalidRoleName(role);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, role);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            var identityError = base.PasswordRequiresUniqueChars(uniqueChars);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, uniqueChars);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError DefaultError()
        {
            var identityError = base.DefaultError();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError ConcurrencyFailure()
        {
            var identityError = base.ConcurrencyFailure();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordTooShort(int length)
        {
            var identityError = base.PasswordTooShort(length);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, length);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            var identityError = base.PasswordRequiresNonAlphanumeric();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordRequiresDigit()
        {
            var identityError = base.PasswordRequiresDigit();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordRequiresLower()
        {
            var identityError = base.PasswordRequiresLower();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordRequiresUpper()
        {
            var identityError = base.PasswordRequiresUpper();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError DuplicateEmail(string email)
        {
            var identityError = base.DuplicateEmail(email);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, email);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            var identityError = base.DuplicateUserName(userName);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, userName);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError InvalidEmail(string email)
        {
            var identityError = base.InvalidEmail(email);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, email);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError InvalidUserName(string userName)
        {
            var identityError = base.InvalidUserName(userName);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, userName);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            var identityError = base.UserAlreadyHasPassword();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            var identityError = base.UserLockoutNotEnabled();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            var identityError = base.UserAlreadyInRole(role);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, role);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError UserNotInRole(string role)
        {
            var identityError = base.UserNotInRole(role);

            var resourceDescription = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}");

            if (!string.IsNullOrEmpty(resourceDescription))
            {
                resourceDescription = string.Format(resourceDescription, role);
            }

            identityError.Description = resourceDescription ?? identityError.Description;
            return identityError;
        }

        public override IdentityError PasswordMismatch()
        {
            var identityError = base.PasswordMismatch();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError InvalidToken()
        {
            var identityError = base.InvalidToken();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            var identityError = base.RecoveryCodeRedemptionFailed();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            var identityError = base.LoginAlreadyAssociated();
            identityError.Description = Resource.ResourceManager.GetString($"IdentityError_{identityError.Code}") ?? identityError.Description;
            return identityError;
        }
    }

}
