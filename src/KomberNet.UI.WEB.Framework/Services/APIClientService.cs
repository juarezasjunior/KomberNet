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
    using KomberNet.Resources;
    using Radzen;
    using Refit;

    public class APIClientService : IAPIClientService
    {
        private readonly NotificationService notificationService;

        public APIClientService(NotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task<TResult> ExecuteHandlingErrorAsync<TResult>(Func<Task<TResult>> operation, Action<KomberNetException> exceptionHandler)
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

                    ShowExceptionMessage(komberNetException.ExceptionCode, komberNetException.AdditionalInfo);
                }
            }

            return default;

            void ShowExceptionMessage(ExceptionCode exceptionCode, string additionalInfo)
            {
                var resourceName = exceptionCode.ToString();
                var resourceValue = Resource.ResourceManager.GetString(resourceName);

                this.notificationService.Notify(new NotificationMessage()
                {
                    Detail = resourceValue,
                });
            }
        }
    }
}
