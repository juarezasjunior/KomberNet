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
    using KomberNet.UI.WEB.Framework.Models;
    using Microsoft.AspNetCore.Components;

    public class InternalNavigationService : IInternalNavigationService
    {
        private readonly NavigationManager navigationManager;

        public InternalNavigationService(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        public Task NavigateToPageAsync(string pageName, params PageParameter[] pageParameters)
        {
            this.navigationManager.NavigateTo($"{pageName}{this.GetParameters(pageParameters)}");

            return Task.CompletedTask;
        }

        private string GetParameters(PageParameter[] pageParameters)
        {
            var result = string.Empty;

            if (pageParameters == null || !pageParameters.Any())
            {
                return result;
            }

            var routeParameters = pageParameters.Where(x => x.PageParameterType == PageParameterType.Route).Select(x => x.GetValue());

            if (routeParameters.Any())
            {
                result = $"/{string.Join("/", routeParameters)}";
            }

            var queryParameters = pageParameters.Where(x => x.PageParameterType == PageParameterType.Query).Select(x => x.GetValue());

            if (queryParameters.Any())
            {
                result += $"?{string.Join("&", queryParameters)}";
            }

            return result;
        }
    }
}
