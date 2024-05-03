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
                this.HandleException(exceptionHandler, showMessage, exception);
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
                this.HandleException(exceptionHandler, showMessage, exception);
            }
        }

        private void HandleException(Action<KomberNetException> exceptionHandler, bool showMessage, ApiException exception)
        {
            if (!string.IsNullOrEmpty(exception.Content))
            {
                KomberNetException komberNetException;

                if (exception.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    komberNetException = JsonSerializer.Deserialize<KomberNetSecurityException>(exception.Content);
                }
                else
                {
                    komberNetException = JsonSerializer.Deserialize<KomberNetException>(exception.Content);
                }

                exceptionHandler?.Invoke(komberNetException);

                if (showMessage)
                {
                    ShowExceptionMessage(komberNetException.ExceptionCode, komberNetException.AdditionalInfo);
                }
            }

            void ShowExceptionMessage(ExceptionCode exceptionCode, string additionalInfo)
            {
                var resourceName = exceptionCode.ToString();
                var resourceValue = Resource.ResourceManager.GetString(resourceName);

                var message = $"{resourceValue}";

                if (string.IsNullOrEmpty(additionalInfo))
                {
                    message += $" {string.Format(Resource.AdditionalInfo, additionalInfo)}";
                }

                this.notificationService.Notify(new NotificationMessage()
                {
                    Detail = message,
                });
            }
        }
    }
}
