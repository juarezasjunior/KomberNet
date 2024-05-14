// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Resources;
    using Radzen;

    public class MessageService : IMessageService
    {
        private readonly NotificationService notificationService;
        private readonly DialogService dialogService;

        public MessageService(
            NotificationService notificationService,
            DialogService dialogService)
        {
            this.notificationService = notificationService;
            this.dialogService = dialogService;
        }

        public Task NotifySuccessAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null) =>
            this.NotifyAsync(NotificationSeverity.Success, message, title, duration, notificationClick, payload);

        public Task NotifyWarningAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null) =>
            this.NotifyAsync(NotificationSeverity.Warning, message, title, duration, notificationClick, payload);

        public Task NotifyErrorAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null) =>
            this.NotifyAsync(NotificationSeverity.Error, message, title, duration, notificationClick, payload);

        public Task NotifyInfoAsync(string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null) =>
            this.NotifyAsync(NotificationSeverity.Info, message, title, duration, notificationClick, payload);

        public async Task ShowErrorMessageAsync(string message, string title = null)
        {
            await this.dialogService.Alert(
                message,
                title ?? Resource.ResourceManager.GetString("Dialog_Error_Title"),
                new AlertOptions() { OkButtonText = Resource.ResourceManager.GetString("Dialog_Error_Button_Ok") });
        }

        public async Task ShowInfoMessageAsync(string message, string title = null)
        {
            await this.dialogService.Alert(
                message,
                title ?? Resource.ResourceManager.GetString("Dialog_Info_Title"),
                new AlertOptions() { OkButtonText = Resource.ResourceManager.GetString("Dialog_Info_Button_Ok") });
        }

        public async Task<bool> ConfirmAsync(string message, string title = null)
        {
            return (await this.dialogService.Confirm(
                message,
                title ?? Resource.ResourceManager.GetString("Dialog_Confirm_Title"),
                new ConfirmOptions()
                {
                    OkButtonText = Resource.ResourceManager.GetString("Dialog_Confirm_Button_Yes"),
                    CancelButtonText = Resource.ResourceManager.GetString("Dialog_Confirm_Button_No"),
                })) ?? false;
        }

        private Task NotifyAsync(NotificationSeverity severity, string message, string title = null, double duration = 10000, Action<object> notificationClick = null, object payload = null)
        {
            this.notificationService.Notify(new NotificationMessage()
            {
                Severity = severity,
                Summary = title,
                Detail = message,
                Duration = duration,
                Click = notificationClick,
                Payload = payload,
            });

            return Task.CompletedTask;
        }
    }
}
