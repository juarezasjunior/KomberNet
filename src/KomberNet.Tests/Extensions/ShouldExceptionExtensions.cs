// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Tests.Extensions
{
    using System.Threading.Tasks;
    using KomberNet.Exceptions;
    using Shouldly;

    public static class ShouldExceptionExtensions
    {
        public static async Task ShouldThrowKomberNetExceptionAsync(this Task task, ExceptionCode exceptionCode, Action<string> additionalInfoValidation = null)
        {
            var exception = await task.ShouldThrowAsync<KomberNetException>();
            exception.ExceptionCode.ShouldBe(exceptionCode);
            additionalInfoValidation?.Invoke(exception.AdditionalInfo);
        }
    }
}
