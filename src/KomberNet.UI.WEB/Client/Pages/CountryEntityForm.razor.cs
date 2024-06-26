﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using KomberNet.Models.General;
    using KomberNet.UI.WEB.Framework.Components;
    using KomberNet.UI.WEB.Framework.Pages;
    using Microsoft.AspNetCore.Components;
    using Radzen;

    public partial class CountryEntityForm : EntityFormPage<CountryHandlerRequest, CountryHandlerResponse, Country, CountryHandlerRequestValidator>
    {
        [Parameter]
        public int Id { get; set; }

        protected override void CreateActionButtons()
        {
            this.ActionButtons.Add(ActionButton.CloseActionButton(this.Localizer, async () => await this.CloseDialogAsync()));
            this.ActionButtons.Add(ActionButton.SaveActionButton(this.Localizer));
        }

        protected override async Task OnExecutingRequestAsync()
        {
            await base.OnExecutingRequestAsync();
        }
    }
}
