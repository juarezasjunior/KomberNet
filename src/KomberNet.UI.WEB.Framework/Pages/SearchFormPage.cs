// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using FluentValidation;
    using KangarooNet.Domain.Entities;
    using Radzen;

    public abstract partial class SearchFormPage<TSummariesQueryRequest, TSummariesQueryResponse, TSummary, TValidator> : FormPage
        where TSummariesQueryRequest : class, ISummariesQueryRequest, new()
        where TSummariesQueryResponse : class, ISummariesQueryResponse<TSummary, ObservableCollection<TSummary>>
        where TSummary : class, ISummary
        where TValidator : AbstractValidator<TSummariesQueryRequest>, new()
    {
        private IDisposable selectedResultsObservable;

        public TSummariesQueryRequest Request { get; } = new TSummariesQueryRequest();

        public ObservableCollection<TSummary> Results { get; private set; } = new ObservableCollection<TSummary>();

        public ObservableCollection<TSummary> SelectedResults { get; private set; } = new ObservableCollection<TSummary>();

        public Subject<TSummary> SelectedSummarySubject { get; } = new Subject<TSummary>();

        public async Task SearchAsync(bool validateRequest = true)
        {
            if (validateRequest)
            {
                await this.ValidateRequestAsync();
            }

            try
            {
                this.Results = (await this.OnSearchingAsync()).Summaries;
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
                this.EnableActionButtons();
            });

            this.EnableActionButtons();
        }

        protected virtual void OnValidatingRequest()
        {
        }

        protected virtual async Task OnValidatingRequestAsync()
        {
            await Task.CompletedTask;
        }

        protected abstract Task<TSummariesQueryResponse> OnSearchingAsync();

        protected override void OnDisposing()
        {
            base.OnDisposing();
            this.selectedResultsObservable?.Dispose();
        }

        private void EnableActionButtons()
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
        }

        private async Task ValidateRequestAsync()
        {
            var validator = new TValidator();
            var validations = (await validator.ValidateAsync(this.Request))?.Errors;

            foreach (var validation in validations)
            {
                this.NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = validation.ErrorMessage,
                });
            }

            this.OnValidatingRequest();
            await this.OnValidatingRequestAsync();
        }
    }
}
