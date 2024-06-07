// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API
{
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using FluentValidation.Results;
    using KomberNet.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class CustomValidatorInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = string.Join(" ", result.Errors.Select(x => x.ErrorMessage));
                throw new KomberNetException(ExceptionCode.RequestValidation, errors);
            }

            return result;
        }
    }
}
