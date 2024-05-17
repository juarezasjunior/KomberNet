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
    using KomberNet.UI.WEB.Framework.Pages;
    using Radzen;
    using Radzen.Blazor.Rendering;

    public interface IInternalNavigationService : IScopedService
    {
        public Task NavigateToPageAsync<TPage>(Dictionary<string, object> routeParameters = null, Dictionary<string, object> queryParameters = null, bool forceLoad = false)
            where TPage : BasePage;

        public Task OpenDialogAsync<TPage>(string title = null, Dictionary<string, object> parameters = null, bool showClose = false, bool isDraggable = true, bool isResizable = true)
            where TPage : BasePage;

        public Task<TResult> OpenDialogAsync<TPage, TResult>(string title = null, Dictionary<string, object> parameters = null, bool showClose = false, bool isDraggable = true, bool isResizable = true)
            where TPage : BasePage;

        public Task CloseDialogAsync();

        public Task CloseDialogAsync<TResult>(TResult result);
    }
}
