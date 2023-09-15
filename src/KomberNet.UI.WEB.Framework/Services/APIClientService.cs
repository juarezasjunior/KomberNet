// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using Radzen;
    using Refit;

    public class APIClientService
    {
        private readonly NotificationService notificationService;

        public APIClientService(NotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        protected async Task<TResult> ExecuteHandlingErrorAsync<TResult>(Func<Task<TResult>> operation, Action<KomberNetException> exceptionHandler)
        {
            try
            {
                var result = await operation?.Invoke();

                return result;
            }
            catch (ApiException exception)
            {
                if (!string.IsNullOrEmpty(exception.Content))
                {
                    if (exception.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        var komberNetSecurityException = JsonSerializer.Deserialize<KomberNetSecurityException>(exception.Content);

                        exceptionHandler?.Invoke(komberNetSecurityException);
                    }
                }
            }

            return default;

            void ShowExceptionMessage(KomberNetException exception)
            {
                this.notificationService.Notify(new NotificationMessage()
                {
                    Detail = exception.ExceptionCode.ToString(),
                });
            }
        }
    }
}
