// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading.Tasks;
    using KangarooNet.Domain.Entities;
    using KomberNet.UI.WEB.Framework.Components;

    public abstract partial class SearchPage<TSummariesQueryRequest, TSummariesQueryResponse, TSummary> : BasePage, IDisposable
        where TSummariesQueryRequest : class, ISummariesQueryRequest, new()
        where TSummariesQueryResponse : class, ISummariesQueryResponse<TSummary, ObservableCollection<TSummary>>
        where TSummary : class, ISummary
    {
        private IDisposable selectedResultsObservable;

        public TSummariesQueryRequest Request { get; } = new TSummariesQueryRequest();

        public ObservableCollection<TSummary> Results { get; private set; } = new ObservableCollection<TSummary>();

        public ObservableCollection<TSummary> SelectedResults { get; private set; } = new ObservableCollection<TSummary>();

        public Subject<TSummary> SelectedSummarySubject { get; } = new Subject<TSummary>();

        public List<ActionButton> ActionButtons { get; } = new List<ActionButton>();

        public void Dispose()
        {
            this.selectedResultsObservable?.Dispose();
            this.OnDisposing();
        }

        public async Task SearchAsync()
        {
            try
            {
                this.Results = (await this.OnSearchAsync()).Summaries;
            }
            catch
            {
                this.Results = new ObservableCollection<TSummary>();
            }

            this.SelectedResults = new ObservableCollection<TSummary>();

            this.StateHasChanged();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.selectedResultsObservable = this.SelectedSummarySubject
            .Throttle(TimeSpan.FromMilliseconds(200))
            .Subscribe(x =>
            {
                foreach (var actionButton in this.ActionButtons)
                {
                    if (actionButton.CanExecute != null)
                    {
                        actionButton.IsEnabled = actionButton.CanExecute.Invoke();
                    }
                    else
                    {
                        actionButton.IsEnabled = true;
                    }
                }

                this.StateHasChanged();
            });
        }

        protected abstract Task<TSummariesQueryResponse> OnSearchAsync();

        protected virtual void OnDisposing()
        {
        }
    }
}
