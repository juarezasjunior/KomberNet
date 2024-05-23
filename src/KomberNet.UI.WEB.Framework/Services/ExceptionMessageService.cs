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
        private readonly IMessageService messageService;

        public ExceptionMessageService(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task<TResult> GetResultOrHandleExceptionAsync<TResult>(Func<Task<TResult>> operation, Func<Exception, Task<bool>> exceptionHandler = null, bool showMessage = true)
        {
            try
            {
                var result = await operation?.Invoke();

                return result;
            }
            catch (Exception exception)
            {
                await this.HandleExceptionAsync(exceptionHandler, showMessage, exception);
            }

            return default;
        }

        public async Task HandleExceptionAsync(Func<Task> operation, Func<Exception, Task<bool>> exceptionHandler = null, bool showMessage = true)
        {
            try
            {
                await operation?.Invoke();
            }
            catch (Exception exception)
            {
                await this.HandleExceptionAsync(exceptionHandler, showMessage, exception);
            }
        }

        private async Task HandleExceptionAsync(Func<Exception, Task<bool>> exceptionHandler, bool showMessage, Exception exception)
        {
            var komberNetException = exception as KomberNetException;

            if (exception is ApiException apiException && !string.IsNullOrEmpty(apiException.Content))
            {
                komberNetException = JsonSerializer.Deserialize<KomberNetException>(apiException.Content);
            }

            var wasHandled = exceptionHandler is not null
                ? await exceptionHandler.Invoke(komberNetException ?? exception)
                : false;

            if (!wasHandled && showMessage)
            {
                if (komberNetException is not null)
                {
                    var exceptionMessage = Resource.ResourceManager.GetString(komberNetException.ExceptionCode.ToString());

                    await ShowExceptionMessageAsync(exceptionMessage, komberNetException.AdditionalInfo);

                    return;
                }

                await ShowExceptionMessageAsync(Resource.ResourceManager.GetString("UnhandledExceptionMessage"), exception.Message);
            }

            async Task ShowExceptionMessageAsync(string message, string additionalInfo)
            {
                if (!string.IsNullOrEmpty(additionalInfo))
                {
                    message += $" {string.Format(Resource.AdditionalInfo, additionalInfo)}";
                }

                await this.messageService.ShowErrorMessageAsync(message);
            }
        }
    }
}
