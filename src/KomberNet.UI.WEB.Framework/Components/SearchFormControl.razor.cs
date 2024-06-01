// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentValidation;
    using KomberNet.Models.Contracts;
    using KomberNet.UI.WEB.Framework.Pages;
    using Microsoft.AspNetCore.Components;

    public partial class SearchFormControl<TSummariesGetRequest, TSummariesGetResponse, TSummary, TValidator> : FormControl
        where TSummariesGetRequest : class, ISummariesGetRequest, new()
        where TSummariesGetResponse : class, ISummariesGetResponse<TSummary, ObservableCollection<TSummary>>
        where TSummary : class, ISummary
        where TValidator : AbstractValidator<TSummariesGetRequest>, new()
    {
        [Parameter]
        public SearchFormPage<TSummariesGetRequest, TSummariesGetResponse, TSummary, TValidator> SearchFormPage { get; set; }

        [Parameter]
        public int? Take { get; set; } = 100;

        [Parameter]
        public string SearchPlaceholder { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment FilterCriteria { get; set; }

        [Parameter]
        public RenderFragment Columns { get; set; }

        [SupplyParameterFromQuery(Name = "Search")]
        private string SearchInputText
        {
            get => (this.SearchFormPage?.Request as IHasSearchableDefaultCriteria)?.Search;
            set
            {
                if (this.SearchFormPage?.Request is IHasSearchableDefaultCriteria defaultCriteria
                    && defaultCriteria.Search != value)
                {
                    defaultCriteria.Search = value;
                    this.SearchFormPage.UpdateQueryString(value, "Search");
                }
            }
        }

        private bool IsShowingMoreFilter { get; set; }

        private bool IsShowingFirstHundredResults { get; set; }

        private string FilterIcon => this.IsShowingMoreFilter ? "expand_less" : "expand_more";

        private string FilterDescription => this.IsShowingMoreFilter ? this.Localizer["Search_ShowLessFilterCriteria"] : this.Localizer["Search_ShowMoreFilterCriteria"];

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.SearchFormPage is null)
            {
                throw new NullReferenceException($"Missing parameter {nameof(this.SearchFormPage)}");
            }
        }

        private async Task SearchAsync()
        {
            if (this.SearchFormPage.Request is IHasSearchableDefaultCriteria defaultCriteria)
            {
                defaultCriteria.Take = this.Take;
            }

            await this.SearchFormPage.SearchAsync();

            if (this.Take.HasValue && this.SearchFormPage.Results.Count() == this.Take.Value)
            {
                this.IsShowingFirstHundredResults = true;
            }
        }

        private async Task RemoveTakeAsync()
        {
            this.IsShowingFirstHundredResults = false;
            this.Take = null;
            await this.SearchAsync();
        }
    }
}
