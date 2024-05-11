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

        public ExceptionMessageService(NotificationService notificationService)
        {
            this.notificationService = notificationService;
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
                this.HandleApiException(exceptionHandler, showMessage, exception);
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
                this.HandleApiException(exceptionHandler, showMessage, exception);
            }
            catch (KomberNetException exception)
            {
                this.HandleKomberNetException(exceptionHandler, showMessage, exception);
            }
        }

        private void HandleApiException(Action<KomberNetException> exceptionHandler, bool showMessage, ApiException exception)
        {
            if (!string.IsNullOrEmpty(exception.Content))
            {
                var komberNetException = JsonSerializer.Deserialize<KomberNetException>(exception.Content);

                this.HandleKomberNetException(exceptionHandler, showMessage, komberNetException);
            }
        }

        private void HandleKomberNetException(Action<KomberNetException> exceptionHandler, bool showMessage, KomberNetException exception)
        {
            exceptionHandler?.Invoke(exception);

            if (showMessage)
            {
                ShowExceptionMessage(exception.ExceptionCode, exception.AdditionalInfo);
            }

            void ShowExceptionMessage(ExceptionCode exceptionCode, string additionalInfo)
            {
                var resourceName = exceptionCode.ToString();
                var resourceValue = Resource.ResourceManager.GetString(resourceName);

                var message = $"{resourceValue}";

                if (!string.IsNullOrEmpty(additionalInfo))
                {
                    message += $" {string.Format(Resource.AdditionalInfo, additionalInfo)}";
                }

                this.notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = message,
                    Duration = 10000,
                });
            }
        }
    }
}
