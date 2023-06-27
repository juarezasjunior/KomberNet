// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Bootstraps
{
    using System.Globalization;
    using Blazored.LocalStorage;
    using KangarooNet.UI.APIClient;
    using KomberNet.UI.WEB.APIClient;
    using KomberNet.UI.WEB.Client.Auth;
    using KomberNet.UI.WEB.Client.Handlers;
    using KomberNet.UI.WEB.Client.Providers;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.JSInterop;
    using Radzen;
    using Refit;

    public static class WEBBootstrap
    {
        public static async Task BootstrapAsync(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            AddRootComponents(builder);

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();

            builder.Services.AddAuthorizationCore();

            AddRefit(builder);

            AddLocalization(builder);

            var host = builder.Build();

            await SetDefaultCulture(host);

            await host.RunAsync();
        }

        private static void AddRootComponents(WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
        }

        private static async Task SetDefaultCulture(WebAssemblyHost host)
        {
            CultureInfo culture;
            var js = host.Services.GetRequiredService<IJSRuntime>();
            var result = await js.InvokeAsync<string>("blazorCulture.get");

            if (result != null)
            {
                culture = new CultureInfo(result);
            }
            else
            {
                culture = new CultureInfo("en-US");
                await js.InvokeVoidAsync("blazorCulture.set", "en-US");
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private static void AddLocalization(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddLocalization();
        }

        private static void AddRefit(WebAssemblyHostBuilder builder)
        {
            var apiOptions = new APIOptions();
            builder.Configuration.GetSection("API").Bind(apiOptions);

            builder.Services.AddTransient<AuthHeaderHandler>();

            var clientInterfaces = typeof(IAuthClient).Assembly.GetTypes().Where(x =>
                x.IsAssignableTo(typeof(IAPIClient))
                    && x.IsInterface);

            foreach (var clientInterface in clientInterfaces)
            {
                builder.Services.AddRefitClient(clientInterface)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiOptions.Url))
                    .AddHttpMessageHandler<AuthHeaderHandler>();
            }
        }

        private class APIOptions
        {
            public string Url { get; set; }
        }
    }
}
