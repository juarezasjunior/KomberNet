// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading.Tasks;
    using KangarooNet.Domain.Entities;
    using Microsoft.AspNetCore.Components;

    public partial class ActionDataGridControl<TItem> : ComponentBase, IDisposable
        where TItem : class
    {
        private IDisposable selectedResultsObservable;
        private Subject<TItem> selectedItemSubject = new Subject<TItem>();

        [Parameter]
        public List<ActionButton> ActionButtons { get; set; } = new List<ActionButton>();

        [Parameter]
        public IEnumerable<TItem> Data { get; set; }

        [Parameter]
        public Action<TItem> OnRowSelected { get; set; }

        [Parameter]
        public Action<TItem> OnRowDeselected { get; set; }

        [Parameter]
        public RenderFragment Columns { get; set; }

        private IList<TItem> InternalSelectedResults { get; set; } = new List<TItem>();

        public void Dispose()
        {
            this.selectedResultsObservable?.Dispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.selectedResultsObservable = this.selectedItemSubject
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(x =>
                {
                    ActionButton.EnableActionButtons(this.ActionButtons);
                    this.StateHasChanged();
                });
        }

        private void OnRowSelect(TItem item)
        {
            this.OnRowSelected?.Invoke(item);
            this.selectedItemSubject.OnNext(item);
        }

        private void OnRowDeselect(TItem item)
        {
            this.OnRowDeselected?.Invoke(item);
            this.selectedItemSubject.OnNext(item);
        }
    }
}
