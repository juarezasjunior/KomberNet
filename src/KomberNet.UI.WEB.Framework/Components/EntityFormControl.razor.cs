// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Components
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Results;
    using KomberNet.Models.Contracts;
    using KomberNet.UI.WEB.Framework.Pages;
    using Microsoft.AspNetCore.Components;

    public partial class EntityFormControl<TEntityHandlerRequest, TEntityHandlerResponse, TEntity, TValidator> : FormControl
        where TEntityHandlerRequest : class, IEntityHandlerRequest<TEntity>, new()
        where TEntity : class, IEntity, new()
        where TEntityHandlerResponse : class, IEntityHandlerResponse, new()
        where TValidator : AbstractValidator<TEntityHandlerRequest>, new()
    {
        [Parameter]
        public EntityFormPage<TEntityHandlerRequest, TEntityHandlerResponse, TEntity, TValidator> EntityFormPage { get; set; }

        [Parameter]
        public RenderFragment Fields { get; set; }

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
