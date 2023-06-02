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

    public partial class EntityForm<TEntityHandlerRequest, TEntityHandlerResponse, TEntity> : BodyBase
        where TEntityHandlerRequest : class, IEntityHandlerRequest<TEntity>, new()
        where TEntity : class, IEntity, new()
        where TEntityHandlerResponse : class, IEntityHandlerResponse<TEntity>, new()
    {
        [Parameter]
        public EntityFormPage<TEntityHandlerRequest, TEntityHandlerResponse, TEntity> EntityFormPage { get; set; }

        [Parameter]
        public RenderFragment FieldsArea { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.EntityFormPage is null)
            {
                throw new NullReferenceException($"Missing parameter {nameof(this.EntityFormPage)}");
            }
        }
    }
}
