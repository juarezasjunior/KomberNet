// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Services
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using KomberNet.Resources;
    using Radzen;
    using Refit;

    public class ExceptionMessageService : IExceptionMessageService
    {
        private readonly NotificationService notificationService;
        private readonly IMessageService messageService;

        public ExceptionMessageService(
            NotificationService notificationService,
            IMessageService messageService)
        {
            this.notificationService = notificationService;
            this.messageService = messageService;
        }

        public async Task<TResult> GetResultOrHandleExceptionAsync<TResult>(Func<Task<TResult>> operation, Action<KomberNetException> exceptionHandler = null, bool showMessage = true)
        {
            try
            {
                var result = await operation?.Invoke();

                return result;
            }
            catch (ApiException exception)
            {
                this.HandleApiExceptionAsync(exceptionHandler, showMessage, exception);
            }

            return default;
        }

        public async Task HandleExceptionAsync(Func<Task> operation, Action<KomberNetException> exceptionHandler = null, bool showMessage = true)
        {
            try
            {
                await operation?.Invoke();
            }
            catch (ApiException exception)
            {
                await this.HandleApiExceptionAsync(exceptionHandler, showMessage, exception);
            }
            catch (KomberNetException exception)
            {
                await this.HandleKomberNetExceptionAsync(exceptionHandler, showMessage, exception);
            }
        }

        private async Task HandleApiExceptionAsync(Action<KomberNetException> exceptionHandler, bool showMessage, ApiException exception)
        {
            if (!string.IsNullOrEmpty(exception.Content))
            {
                var komberNetException = JsonSerializer.Deserialize<KomberNetException>(exception.Content);

                await this.HandleKomberNetExceptionAsync(exceptionHandler, showMessage, komberNetException);
            }
        }

        private async Task HandleKomberNetExceptionAsync(Action<KomberNetException> exceptionHandler, bool showMessage, KomberNetException exception)
        {
            exceptionHandler?.Invoke(exception);

            if (showMessage)
            {
                await ShowExceptionMessageAsync(exception.ExceptionCode, exception.AdditionalInfo);
            }

            async Task ShowExceptionMessageAsync(ExceptionCode exceptionCode, string additionalInfo)
            {
                var resourceName = exceptionCode.ToString();
                var resourceValue = Resource.ResourceManager.GetString(resourceName);

                var message = $"{resourceValue}";

                if (!string.IsNullOrEmpty(additionalInfo))
                {
                    message += $" {string.Format(Resource.AdditionalInfo, additionalInfo)}";
                }

                await this.messageService.ShowErrorMessageAsync(message);
            }
        }
    }
}
