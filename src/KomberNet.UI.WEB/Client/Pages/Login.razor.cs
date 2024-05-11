// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using KomberNet.Models.Auth;

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
    }
}
