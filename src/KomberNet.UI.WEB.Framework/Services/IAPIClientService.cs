// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.Exceptions;

    public interface IAPIClientService
    {
        public Task<TResult> ExecuteHandlingErrorAsync<TResult>(Func<Task<TResult>> operation, Action<KomberNetException> exceptionHandler);
    }
}
