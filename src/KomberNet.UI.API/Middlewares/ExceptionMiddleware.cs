// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Middlewares
{
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (KomberNetException exception)
            {
                this.logger.LogError(exception.ToString());

                if (exception.ExceptionCode == ExceptionCode.SecurityValidation)
                {
                    await this.HandleExceptionAsync(context, exception.ExceptionCode, httpStatusCode: HttpStatusCode.Unauthorized);
                    return;
                }

                await this.HandleExceptionAsync(context, exception.ExceptionCode, exception.AdditionalInfo);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception.ToString());
                await this.HandleExceptionAsync(context);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, ExceptionCode exceptionCode = ExceptionCode.Others, string? additionalInfo = null, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(new ExceptionDetail()
                {
                    ExceptionCode = exceptionCode,
                    AdditionalInfo = additionalInfo,
                }));
        }
    }
}
