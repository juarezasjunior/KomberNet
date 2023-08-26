// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using System.Collections.ObjectModel;
    using KomberNet.Models.General;
    using KomberNet.UI.WEB.Framework.Components;
    using KomberNet.UI.WEB.Framework.Pages;
    using Radzen;

    public partial class CountriesSearchForm : SearchFormPage<CountrySummariesQueryRequest, CountrySummariesQueryResponse, CountrySummary, CountrySummariesQueryRequestValidator>
    {
        protected override void CreateActionButtons()
        {
            this.ActionButtons.Add(ActionButton.NewActionButton(this.Localizer, () => true, async () => await this.NewCountry()));
            this.ActionButtons.Add(ActionButton.OpenActionButton(this.Localizer, () => this.SelectedResults.Count() >= 2, () => Console.WriteLine("On Open")));
        }

        protected override Task<CountrySummariesQueryResponse> OnSearchingAsync()
        {
            var countries = new ObservableCollection<CountrySummary>();
            countries.Add(new CountrySummary() { CountryId = Guid.NewGuid(), Name = "Brazil" });
            countries.Add(new CountrySummary() { CountryId = Guid.NewGuid(), Name = "EUA" });
            countries.Add(new CountrySummary() { CountryId = Guid.NewGuid(), Name = "Mexico" });
            countries.Add(new CountrySummary() { CountryId = Guid.NewGuid(), Name = "Japan" });

            return Task.FromResult(new CountrySummariesQueryResponse()
            {
                Summaries = countries,
            });
        }

        private async Task NewCountry()
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("Id", 1);

            await this.DialogService.OpenAsync<CountryEntityForm>("Test Country", parameters, new DialogOptions()
            {
                ShowTitle = false,
                ShowClose = false,
                Draggable = true,
                Resizable = true,
                CloseDialogOnEsc = false,
                //Width = "700px",
                //Height = "512px",
            });
        }

        public class Customer
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
