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
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using KangarooNet.Domain.Entities;
    using KomberNet.UI.WEB.Framework.Components;

    public abstract partial class EntityFormPage<TEntityHandlerRequest, TEntityHandlerResponse, TEntity> : BodyBase
        where TEntityHandlerRequest : class, IEntityHandlerRequest<TEntity>, new()
        where TEntity : class, IEntity, new()
        where TEntityHandlerResponse : class, IEntityHandlerResponse<TEntity>, new()
    {
        public TEntityHandlerRequest Request { get; } = new TEntityHandlerRequest() { Entity = new TEntity() };

        public TEntity Entity => this.Request.Entity;

        public bool IsBusy { get; protected set; }

        public List<ActionButton> ActionButtons { get; } = new List<ActionButton>();

        public void Dispose()
        {
            this.OnDisposing();
        }

        protected async Task ExecuteRequestAsync(bool validateRequest = true)
        {
            this.IsBusy = true;

            this.OnExecuteRequest();
            await this.OnExecuteRequestAsync();

            this.IsBusy = false;
        }

        protected virtual void OnExecuteRequest()
        {
        }

        protected virtual async Task OnExecuteRequestAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual void OnDisposing()
        {
        }
    }
}
