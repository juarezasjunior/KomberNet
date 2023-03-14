// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Pages
{
    using System.Collections.ObjectModel;
    using KomberNet.UI.WEB.Models;

    public partial class CountriesSearchPage
    {
        protected override Task<CountrySummariesQueryResponse> OnSearchAsync()
        {
            return Task.FromResult(new CountrySummariesQueryResponse()
            {
                Summaries = new ObservableCollection<CountrySummary>()
                {
                    new CountrySummary() { CountryId = Guid.NewGuid(), Name = "Brazil" },
                },
            });
        }
    }
}
