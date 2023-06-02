// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using System.Collections.ObjectModel;
    using KomberNet.UI.WEB.Framework.Components;
    using KomberNet.UI.WEB.Framework.Pages;
    using KomberNet.UI.WEB.Models;
    using Microsoft.AspNetCore.Components;
    using Radzen;

    public partial class CountryEntityFormPage : EntityFormPage<CountryHandlerRequest, CountryHandlerResponse, Country>
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public DialogService DialogService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.ActionButtons.Add(ActionButton.CloseActionButton(this.Localizer, () => true, () => this.DialogService.Close()));
            this.ActionButtons.Add(ActionButton.SaveActionButton(this.Localizer, () => true, async () => await this.ExecuteRequestAsync()));
        }

        protected override Task OnExecuteRequestAsync()
        {
            return base.OnExecuteRequestAsync();
        }
    }
}
