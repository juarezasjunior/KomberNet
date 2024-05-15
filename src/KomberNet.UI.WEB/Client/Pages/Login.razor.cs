// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using KomberNet.Exceptions;
    using KomberNet.Models.Auth;
    using KomberNet.Resources;

    public partial class Login
    {
        private string Email { get; set; }

        private string Password { get; set; }

        private async Task LoginAsync()
        {
            var isLogged = true;

            await this.HandleExceptionAsync(
                async () =>
                {
                    await this.userService.LoginAsync(new LoginRequest()
                    {
                        Email = this.Email,
                        Password = this.Password,
                    });
                },
                x => isLogged = false);

            if (isLogged)
            {
                await this.NavigateToPageAsync<Index>();
            }
        }

        private async Task OpenNewUserAsync() => await this.NavigateToPageAsync<NewUser>();
    }
}
