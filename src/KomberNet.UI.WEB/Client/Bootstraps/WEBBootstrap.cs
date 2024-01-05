// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.WEB.Client.Bootstraps
{
    using System.Globalization;
    using System.Reflection;
    using Blazored.LocalStorage;
    using KomberNet.UI.WEB.APIClient;
    using KomberNet.UI.WEB.APIClient.Auth;
    using KomberNet.UI.WEB.Client.Auth;
    using KomberNet.UI.WEB.Client.Handlers;
    using KomberNet.UI.WEB.Client.Providers;
    using KomberNet.UI.WEB.Client.Shared;
    using KomberNet.UI.WEB.Framework.Services;
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

            builder.Services.AddServices();

            builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddSingleton<DialogService>();
            builder.Services.AddSingleton<NotificationService>();

            builder.Services.AddAuthorizationCore();

            AddRefit(builder);

            AddLocalization(builder);

            var host = builder.Build();

            await SetTheme(host);
            await SetDefaultCulture(host);

            await host.RunAsync();
        }

        private static void AddRootComponents(WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
        }

        private static async Task SetTheme(WebAssemblyHost host)
        {
            var js = host.Services.GetRequiredService<IJSRuntime>();
            var theme = await js.InvokeAsync<string>("theme.get");

            if (string.IsNullOrEmpty(theme))
            {
                theme = ThemeSelector.SupportedThemes.FirstOrDefault();
            }

            await js.InvokeVoidAsync("setTheme", theme);
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

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Since AddBlazoredLocalStorage is using Scoped instead of Singleton, we cannot add the services
            // with a singleton lifetime. For this reason, they are scoped as well.
            return services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<IScopedService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        private static void AddRefit(WebAssemblyHostBuilder builder)
        {
            var apiOptions = new APIOptions();
            builder.Configuration.GetSection("API").Bind(apiOptions);

            builder.Services.AddTransient<MessageHandler>();
            builder.Services.AddTransient<AuthMessageHandler>();

            var test = GetAPIClientAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault();

            var anonymousClientInterfaces = GetAPIClientAssemblies().SelectMany(x => x.GetTypes()).Where(x =>
                x.IsAssignableTo(typeof(IAnonymousAPIClient))
                    && x.IsInterface);

            foreach (var clientInterface in anonymousClientInterfaces)
            {
                builder.Services.AddRefitClient(clientInterface)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiOptions.Url))
                    .AddHttpMessageHandler<MessageHandler>();
            }

            var authenticatedClientInterfaces = GetAPIClientAssemblies().SelectMany(x => x.GetTypes()).Where(x =>
                x.IsAssignableTo(typeof(IAuthenticatedAPIClient))
                    && x.IsInterface);

            foreach (var clientInterface in authenticatedClientInterfaces)
            {
                builder.Services.AddRefitClient(clientInterface)
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiOptions.Url))
                    .AddHttpMessageHandler<AuthMessageHandler>();
            }
        }

        private static IEnumerable<Assembly> GetServiceAssemblies()
        {
            return new[]
            {
                Assembly.Load("KomberNet.UI.WEB.Client"),
                Assembly.Load("KomberNet.UI.WEB.Billing"),
                Assembly.Load("KomberNet.UI.WEB.Financial"),
                Assembly.Load("KomberNet.UI.WEB.Framework"),
                Assembly.Load("KomberNet.UI.WEB.Inventory"),
                Assembly.Load("KomberNet.UI.WEB.Manufacturing"),
                Assembly.Load("KomberNet.UI.WEB.Purchasing"),
                Assembly.Load("KomberNet.UI.WEB.Shared"),
            };
        }

        private static IEnumerable<Assembly> GetAPIClientAssemblies()
        {
            return new[]
            {
                Assembly.Load("KomberNet.UI.WEB.APIClient"),
                Assembly.Load("KomberNet.UI.WEB.APIClient.Billing"),
                Assembly.Load("KomberNet.UI.WEB.APIClient.Financial"),
                Assembly.Load("KomberNet.UI.WEB.APIClient.Inventory"),
                Assembly.Load("KomberNet.UI.WEB.APIClient.Manufacturing"),
                Assembly.Load("KomberNet.UI.WEB.APIClient.Purchasing"),
            };
        }

        private class APIOptions
        {
            public string Url { get; set; }
        }
    }
}
