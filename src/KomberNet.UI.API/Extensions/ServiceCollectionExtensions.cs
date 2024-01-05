// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Extensions
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Models.Auth;
    using KomberNet.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<ITransientService>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<IScopedService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<ISingletonService>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return services;
        }

        public static IServiceCollection AddDatabaseRepositories(this IServiceCollection services)
        {
            return services.Scan(x =>
                x.FromAssemblies(typeof(ApplicationDbContext).Assembly)
                .AddClasses(y =>
                    y.AssignableTo(typeof(IDatabaseRepository<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }

        public static IServiceCollection AddAuthenticationJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.GetSection(JwtOptions.Jwt).Bind(jwtOptions);

            var validIssuer = jwtOptions.JwtIssuer;
            var validAudience = jwtOptions.JwtAudience;
            var secretKey = jwtOptions.JwtSecurityKey;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = validIssuer,
                        ValidAudience = validAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    };
                });

            return services;
        }

        private static IEnumerable<Assembly> GetServiceAssemblies()
        {
            return new[]
            {
                Assembly.Load("KomberNet.Services"),
                Assembly.Load("KomberNet.Services.Billing"),
                Assembly.Load("KomberNet.Services.Financial"),
                Assembly.Load("KomberNet.Services.Inventory"),
                Assembly.Load("KomberNet.Services.Manufacturing"),
                Assembly.Load("KomberNet.Services.Purchasing"),
            };
        }
    }
}
