// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using Microsoft.AspNetCore.Components;

    public partial class NewUser
    {
        private string FullName { get; set; }

        private string Email { get; set; }

        private string Password { get; set; }

        private string RepeatPassword { get; set; }

        private async Task InsertUserAsync()
        {
            var userInserted = true;

            await this.HandleExceptionAsync(
                async () =>
                {
                    if (this.Password != this.RepeatPassword)
                    {
                        throw new KomberNetException(ExceptionCode.Auth_User_PasswordsNotIdentical);
                    }

                    await this.userService.InsertUserAsync(new UserInsertRequest()
                    {
                        FullName = this.FullName,
                        Email = this.Email,
                        Password = this.Password,
                    });
                },
                x => userInserted = false);

            if (userInserted)
            {
                await this.NavigateToPageAsync<Login>();
            }
        }
    }
}
