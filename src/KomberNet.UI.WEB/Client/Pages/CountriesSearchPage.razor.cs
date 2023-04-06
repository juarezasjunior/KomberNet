// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using System.Collections.ObjectModel;
    using KomberNet.UI.WEB.Framework.Components;
    using KomberNet.UI.WEB.Framework.Pages;
    using KomberNet.UI.WEB.Models;

    public partial class CountriesSearchPage : SearchPage<CountrySummariesQueryRequest, CountrySummariesQueryResponse, CountrySummary>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.ActionButtons.Add(ActionButton.NewActionButton(this.Localizer, () => true, () => Console.WriteLine("On New")));
            this.ActionButtons.Add(ActionButton.OpenActionButton(this.Localizer, () => this.SelectedResults.Count() >= 2, () => Console.WriteLine("On Open")));
        }

        protected override Task<CountrySummariesQueryResponse> OnSearchAsync()
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

        public class Customer
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
