// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Bootstraps
{
    using System.Configuration;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Infrastructure.DatabaseRepositories.Mapper;
    using KomberNet.Models.Auth;
    using KomberNet.UI.API.ActionFilters;
    using KomberNet.UI.API.Extensions;
    using KomberNet.UI.API.Middlewares;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;

    public static class APIBootstrap
    {
        private const string KomberNetCORSOrigins = "KomberNetCORSOrigins";

        public static async Task ConfigureAPIAsync(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddServices();
            builder.Services.AddDatabaseRepositories(typeof(ApplicationDbContext).Assembly);
            builder.Services.AddAuthenticationJwt(builder.Configuration);
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddIdentityCore<TbApplicationUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var connectionStringsOptions = new ConnectionStringsOptions();
            builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStringsOptions);

            builder.Services.AddDbContext<ApplicationDbContext>(x =>
                x.UseSqlServer(
                    connectionStringsOptions.DefaultConnection,
                    y => y.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            var corsOptions = new CorsOptions();
            builder.Configuration.GetSection("Cors").Bind(corsOptions);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    KomberNetCORSOrigins,
                    policy =>
                    {
                        policy.WithOrigins(corsOptions.Origin).AllowAnyHeader().AllowAnyMethod();
                    });
            });

            builder.Services.AddLogging(x => x.AddDebug())
                .AddAutoMapper(typeof(ApplicationAutoMapperProfile))
                .AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddMvc(x =>
            {
                x.Filters.Add(typeof(AuthorizationActionFilter));
                x.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            builder.Services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[]
                        {
                        }
                    },
                });
            });

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.Jwt));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>(Array.Empty<object>());

            app.UseHttpsRedirection();

            app.UseCors(KomberNetCORSOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            await ConfigureDatabase(app);

            app.Run();
        }

        private static async Task ConfigureDatabase(WebApplication app)
        {
            if (app is IApplicationBuilder applicationBuilder)
            {
                using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    var myDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                    await myDbContext.Database.EnsureDeletedAsync();
                    await myDbContext.Database.EnsureCreatedAsync();
                }
            }
        }

        private class ConnectionStringsOptions
        {
            public string DefaultConnection { get; set; }
        }

        private class CorsOptions
        {
            public string Origin { get; set; }
        }
    }
}
