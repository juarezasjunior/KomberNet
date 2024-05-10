// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using KomberNet.Models.Auth;
    using KomberNet.UI.WEB.Framework.Models;

    public partial class Login
    {
        private string Email { get; set; }

        private string Password { get; set; }

        private async Task LoginAsync()
        {
            await this.userService.InsertUserAsync(new UserInsertRequest()
            {
                FullName = this.Email,
                Email = this.Email,
                Password = this.Password,
            });

            var result = await this.userService.LoginAsync(new LoginRequest()
            {
                Email = this.Email,
                Password = this.Password,
            });
        }

        private async Task OpenNewUserAsync()
        {
            await this.NavigateToPageAsync(nameof(NewUser), new PageParameter(nameof(NewUser.Id), 15), new PageParameter(nameof(NewUser.FullName), "My test name", PageParameterType.Query));
        }
    }
}
