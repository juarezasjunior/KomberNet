// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using KomberNet.UI.WEB.Framework.Pages;
    using Microsoft.AspNetCore.Components;
    using Radzen;

    public class InternalNavigationService : IInternalNavigationService
    {
        private readonly NavigationManager navigationManager;
        private readonly DialogService dialogService;

        public InternalNavigationService(
            NavigationManager navigationManager,
            DialogService dialogService)
        {
            this.navigationManager = navigationManager;
            this.dialogService = dialogService;
        }

        public async Task<dynamic> OpenDialogAsync<TPage>(string title, Dictionary<string, object> parameters = null, DialogOptions options = null)
            where TPage : BasePage
        {
            return await this.dialogService.OpenAsync<TPage>(title, parameters, options);
        }

        public Task NavigateToPageAsync<TPage>(Dictionary<string, object> routeParameters = null, Dictionary<string, object> queryParameters = null)
            where TPage : BasePage
        {
            this.navigationManager.NavigateTo($"{typeof(TPage).Name}{this.GetParameters(routeParameters, queryParameters)}");

            return Task.CompletedTask;
        }

        private string GetParameters(Dictionary<string, object> routeParameters = null, Dictionary<string, object> queryParameters = null)
        {
            var result = string.Empty;

            if (routeParameters != null && routeParameters.Any())
            {
                result = $"/{string.Join("/", routeParameters.Select(x => this.GetRouteParameterValue(x.Value.ToString())))}";
            }

            if (queryParameters != null && queryParameters.Any())
            {
                result += $"?{string.Join("&", queryParameters.Select(x => this.GetQueryParameterValue(x.Key, x.Value.ToString())))}";
            }

            return result;
        }

        private string GetRouteParameterValue(string value) => Uri.EscapeDataString(value);

        private string GetQueryParameterValue(string name, string value) => $"{name}={Uri.EscapeDataString(value)}";
    }
}
