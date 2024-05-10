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

    public interface IInternalNavigationService : IScopedService
    {
        public Task<dynamic> OpenDialogAsync<TPage>(string title, Dictionary<string, object> parameters = null, DialogOptions options = null)
            where TPage : BasePage;

        public Task NavigateToPageAsync<TPage>(Dictionary<string, object> routeParameters = null, Dictionary<string, object> queryParameters = null)
            where TPage : BasePage;
    }
}
