﻿// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Resources;
    using KomberNet.UI.WEB.Framework.Services;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Localization;
    using Radzen;

    public abstract partial class BasePage : ComponentBase, IDisposable
    {
        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        private IAPIClientService APIClientService { get; set; }

        public void Dispose()
        {
            this.OnDisposing();
        }

        protected virtual void OnDisposing()
        {
        }

        protected async Task<TResult> ExecuteHandlingErrorAsync<TResult>(Func<Task<TResult>> operation, Action<KomberNetException> exceptionHandler)
        {
            return await this.APIClientService.ExecuteHandlingErrorAsync(operation, exceptionHandler);
        }
    }
}
