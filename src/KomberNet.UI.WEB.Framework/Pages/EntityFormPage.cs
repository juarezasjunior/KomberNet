// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System.Threading.Tasks;
    using FluentValidation;
    using KomberNet.Exceptions;
    using KomberNet.Models.Contracts;
    using KomberNet.Resources;
    using Radzen;

    public abstract partial class EntityFormPage<TEntityHandlerRequest, TEntityHandlerResponse, TEntity, TValidator> : FormPage
        where TEntityHandlerRequest : class, IEntityHandlerRequest<TEntity>, new()
        where TEntity : class, IEntity, new()
        where TEntityHandlerResponse : class, IEntityHandlerResponse, new()
        where TValidator : AbstractValidator<TEntityHandlerRequest>, new()
    {
        public TEntityHandlerRequest Request { get; } = new TEntityHandlerRequest() { Entity = new TEntity() };

        public TEntity Entity => this.Request.Entity;

        public async Task ExecuteRequestAsync(bool validateRequest = true)
        {
            this.IsBusy = true;

            if (validateRequest)
            {
                await this.ValidateRequestAsync();
            }

            this.OnExecutingRequest();
            await this.OnExecutingRequestAsync();

            this.IsBusy = false;
        }

        protected virtual void OnValidatingRequest()
        {
        }

        protected virtual async Task OnValidatingRequestAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual void OnExecutingRequest()
        {
        }

        protected virtual async Task OnExecutingRequestAsync()
        {
            await Task.CompletedTask;
        }

        private async Task ValidateRequestAsync()
        {
            await this.HandleExceptionAsync(
                async () =>
                {
                    var validator = new TValidator();
                    var validations = (await validator.ValidateAsync(this.Request))?.Errors;

                    if (validations is null)
                    {
                        return;
                    }

                    var additionalInfo = string.Join(", ", validations.Select(v => v.ErrorMessage));

                    throw new KomberNetException(ExceptionCode.RequestValidation, additionalInfo);
                });

            this.OnValidatingRequest();
            await this.OnValidatingRequestAsync();
        }
    }
}
