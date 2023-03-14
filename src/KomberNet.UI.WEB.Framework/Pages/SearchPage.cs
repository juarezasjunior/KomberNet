// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KangarooNet.Domain.Entities;

    public abstract partial class SearchPage<TSummariesQueryRequest, TSummariesQueryResponse, TSummary> : BasePage
        where TSummariesQueryRequest : ISummariesQueryRequest
        where TSummary : class, ISummary
        where TSummariesQueryResponse : class, ISummariesQueryResponse<TSummary, ObservableCollection<TSummary>>
    {
        public ObservableCollection<TSummary> Results { get; set; }

        public TSummary SelectedResult { get; set; }

        protected abstract Task<TSummariesQueryResponse> OnSearchAsync();

        private async Task SearchAsync()
        {
            try
            {
                this.Results = (await this.OnSearchAsync()).Summaries;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
