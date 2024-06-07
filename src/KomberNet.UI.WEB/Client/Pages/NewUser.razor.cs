// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using KomberNet.Resources;

    public partial class NewUser
    {
        private string FullName { get; set; }

        private string Email { get; set; }

        private string Password { get; set; }

        private string RepeatPassword { get; set; }

        private async Task CreateUserAsync()
        {
            var userInserted = true;

            await this.HandleExceptionAsync(
                async () =>
                {
                    if (this.Password != this.RepeatPassword)
                    {
                        throw new KomberNetException(ExceptionCode.Auth_User_PasswordsNotIdentical);
                    }

                    await this.userService.CreateUserAsync(new CreateUserRequest()
                    {
                        FullName = this.FullName,
                        Email = this.Email,
                        Password = this.Password,
                    });
                },
                exception =>
                {
                    userInserted = false;

                    return Task.FromResult(false);
                });

            if (userInserted)
            {
                await this.NotifySuccessAsync(Resource.Auth_User_UserInsertedMessage);
                await this.NavigateToPageAsync<Login>();
            }
        }
    }
}
