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

    public interface IExceptionMessageService : IScopedService
    {
        /// <summary>
        /// It's going to try some operation that returns a result and if it fails, it's going to handle the exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the result awaited.</typeparam>
        /// <param name="operation">Operation that should be tried.</param>
        /// <param name="exceptionHandler">In case of handle some exception. You should return if the operation was handled or not. When it's handled, the exception message will not be displayed.</param>
        /// <param name="showMessage">Should show the message or not.</param>
        /// <returns>The awaited result or null if it fails.</returns>
        public Task<TResult> GetResultOrHandleExceptionAsync<TResult>(Func<Task<TResult>> operation, Func<Exception, Task<bool>> exceptionHandler = null, bool showMessage = true);

        /// <summary>
        /// It's going to try some operation and if it fails, it's going to handle the exception.
        /// </summary>
        /// <param name="operation">Operation that should be tried.</param>
        /// <param name="exceptionHandler">In case of handle some exception. You should return if the operation was handled or not. When it's handled, the exception message will not be displayed.</param>
        /// <param name="showMessage">Should show the message or not.</param>
        /// <returns>Task.</returns>
        public Task HandleExceptionAsync(Func<Task> operation, Func<Exception, Task<bool>> exceptionHandler = null, bool showMessage = true);
    }
}
