// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using FluentValidation;
    using KomberNet.Models.Contracts;
    using Radzen;

    public abstract partial class SearchFormPage<TSummariesQueryRequest, TSummariesQueryResponse, TSummary, TValidator> : FormPage
        where TSummariesQueryRequest : class, ISummariesQueryRequest, new()
        where TSummariesQueryResponse : class, ISummariesQueryResponse<TSummary, ObservableCollection<TSummary>>
        where TSummary : class, ISummary
        where TValidator : AbstractValidator<TSummariesQueryRequest>, new()
    {
        public TSummariesQueryRequest Request { get; } = new TSummariesQueryRequest();

        public ObservableCollection<TSummary> Results { get; private set; } = new ObservableCollection<TSummary>();

        public ObservableCollection<TSummary> SelectedResults { get; set; } = new ObservableCollection<TSummary>();

        public async Task SearchAsync(bool validateRequest = true)
        {
            this.IsBusy = true;

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

            this.IsBusy = false;

            this.StateHasChanged();
        }

        public virtual void OnResultSelected(TSummary summary)
        {
            this.SelectedResults.Add(summary);
        }

        public virtual void OnResultDeselected(TSummary summary)
        {
            this.SelectedResults.Remove(summary);
        }

        protected virtual void OnValidatingRequest()
        {
        }

        protected virtual async Task OnValidatingRequestAsync()
        {
            await Task.CompletedTask;
        }

        protected abstract Task<TSummariesQueryResponse> OnSearchingAsync();

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
