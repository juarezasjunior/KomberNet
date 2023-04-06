// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KangarooNet.Domain.Entities;
    using KomberNet.UI.WEB.Framework.Pages;
    using Microsoft.AspNetCore.Components;

    public partial class Search<TSummariesQueryRequest, TSummariesQueryResponse, TSummary> : BodyBase
        where TSummariesQueryRequest : class, ISummariesQueryRequest, new()
        where TSummary : class, ISummary
        where TSummariesQueryResponse : class, ISummariesQueryResponse<TSummary, ObservableCollection<TSummary>>
    {
        private IList<TSummary> InternalSelectedResults { get; set; } = new List<TSummary>();

        [Parameter]
        public SearchPage<TSummariesQueryRequest, TSummariesQueryResponse, TSummary> SearchPage { get; set; }

        [Parameter]
        public int? Take { get; set; } = 100;

        [Parameter]
        public string SearchPlaceholder { get; set; } = string.Empty;

        [Parameter]
        public bool ShouldSearchFirstHundredResults { get; set; } = true;

        [Parameter]
        public RenderFragment FilterCriteriaArea { get; set; }

        [Parameter]
        public RenderFragment SearchColumnsArea { get; set; }

        private string SearchInputText { get; set; }

        private bool IsShowingMoreFilter { get; set; }

        private bool IsShowingFirstHundredResults { get; set; }

        private string FilterIcon => this.IsShowingMoreFilter ? "expand_less" : "expand_more";

        private string FilterDescription => this.IsShowingMoreFilter ? this.Localizer["Search_ShowLessFilterCriteria"] : this.Localizer["Search_ShowMoreFilterCriteria"];

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.SearchPage is null)
            {
                throw new NullReferenceException($"Missing parameter {nameof(this.SearchPage)}");
            }
        }

        private void OnRowSelect(TSummary summary)
        {
            this.SearchPage.SelectedResults.Add(summary);
            this.SearchPage.SelectedSummarySubject.OnNext(summary);
        }

        private void OnRowDeselect(TSummary summary)
        {
            this.SearchPage.SelectedResults.Remove(summary);
            this.SearchPage.SelectedSummarySubject.OnNext(summary);
        }

        private async Task SearchAsync()
        {
            if (this.SearchPage.Request is IHasSearchableDefaultCriteria defaultCriteria)
            {
                defaultCriteria.Search = this.SearchInputText;
                defaultCriteria.Take = this.Take;
            }

            await this.SearchPage.SearchAsync();

            if (this.Take.HasValue && this.SearchPage.Results.Count() == this.Take.Value)
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
