// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
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
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IInternalNavigationService InternalNavigationService { get; set; }

        [Inject]
        private IExceptionMessageService ExceptionMessageService { get; set; }

        [Inject]
        private IMessageService MessageService { get; set; }

        public void Dispose()
        {
            this.OnDisposing();
        }

        public void UpdateQueryString(string value, [CallerMemberName] string name = null)
        {
            var uri = this.NavigationManager.GetUriWithQueryParameter(name, value);
            this.NavigationManager.NavigateTo(uri);
        }

        protected virtual void OnDisposing()
        {
        }

        protected async Task NavigateToPageAsync<TPage>(Dictionary<string, object> routeParameters = null, Dictionary<string, object> queryParameters = null)
            where TPage : BasePage
        {
            await this.InternalNavigationService.NavigateToPageAsync<TPage>(routeParameters, queryParameters);
        }

        protected async Task OpenDialogAsync<TPage>(string title = null, Dictionary<string, object> parameters = null, bool showClose = false, bool isDraggable = true, bool isResizable = true)
            where TPage : BasePage
        {
            await this.InternalNavigationService.OpenDialogAsync<TPage>(title, parameters, showClose, isDraggable, isResizable);
        }

        protected async Task<TResult> OpenDialogAsync<TPage, TResult>(string title = null, Dictionary<string, object> parameters = null, bool showClose = false, bool isDraggable = true, bool isResizable = true)
            where TPage : BasePage
        {
            return await this.InternalNavigationService.OpenDialogAsync<TPage, TResult>(title, parameters, showClose, isDraggable, isResizable);
        }

        protected async Task CloseDialogAsync()
        {
            await this.InternalNavigationService.CloseDialogAsync();
        }

        protected async Task CloseDialogAsync<TResult>(TResult result)
        {
            await this.InternalNavigationService.CloseDialogAsync(result);
        }

        protected async Task<TResult> GetResultOrHandleExceptionAsync<TResult>(Func<Task<TResult>> operation, Action<KomberNetException> exceptionHandler = null, bool showMessage = true)
        {
            return await this.ExceptionMessageService.GetResultOrHandleExceptionAsync(operation, exceptionHandler, showMessage);
        }

        protected async Task HandleExceptionAsync(Func<Task> operation, Action<KomberNetException> exceptionHandler = null, bool showMessage = true)
        {
            await this.ExceptionMessageService.HandleExceptionAsync(operation, exceptionHandler, showMessage);
        }

        protected async Task NotifySuccessAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null)
        {
            await this.MessageService.NotifySuccessAsync(message, title, duration, notificationClick, payload);
        }

        protected async Task NotifyWarningAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null)
        {
            await this.MessageService.NotifyWarningAsync(message, title, duration, notificationClick, payload);
        }

        protected async Task NotifyErrorAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null)
        {
            await this.MessageService.NotifyErrorAsync(message, title, duration, notificationClick, payload);
        }

        protected async Task NotifyInfoAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null)
        {
            await this.MessageService.NotifyInfoAsync(message, title, duration, notificationClick, payload);
        }

        protected async Task ShowErrorMessageAsync(string message, string title = null)
        {
            await this.MessageService.ShowErrorMessageAsync(message, title);
        }

        protected async Task ShowInfoMessageAsync(string message, string title = null)
        {
            await this.MessageService.ShowInfoMessageAsync(message, title);
        }

        protected async Task<bool> ConfirmAsync(string message, string title = null)
        {
            return await this.MessageService.ConfirmAsync(message, title);
        }
    }
}
