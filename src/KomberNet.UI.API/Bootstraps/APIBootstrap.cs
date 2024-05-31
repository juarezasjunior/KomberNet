// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.UI.API.Bootstraps
{
    using System.Configuration;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using KomberNet.Contracts;
    using KomberNet.Infrastructure.DatabaseRepositories;
    using KomberNet.Infrastructure.DatabaseRepositories.Entities.Auth;
    using KomberNet.Models.Auth;
    using KomberNet.Services;
    using KomberNet.Services.Auth;
    using KomberNet.UI.API.ActionFilters;
    using KomberNet.UI.API.Identity;
    using KomberNet.UI.API.Middlewares;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;

    public static class APIBootstrap
    {
        private const string KomberNetCORSOrigins = "KomberNetCORSOrigins";

        public static async Task ConfigureAPIAsync(WebApplicationBuilder builder)
        {
            var mvcBuilder = builder.Services.AddControllers();

            ConfigureSwagger(builder);

            ConfigureLocalization(builder, mvcBuilder);

            ConfigureServices(builder);
            ConfigureDatabase(builder);
            ConfigureAutoMapper(builder);

            ConfigureCache(builder);

            ConfigureJWT(builder);
            ConfigureCORS(builder);

            ConfigureLogging(builder);

            ConfigureValidations(builder);
            ConfigureMVC(builder);

            var app = ConfigureMiddlewares(builder);

            await StartDatabase(app);

            app.Run();
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();

            // TODO: Configure user authentication to access Swagger
            builder.Services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Accept-Language", new OpenApiSecurityScheme
                {
                    Description = "Accept-Language header",
                    Name = "Accept-Language",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "apiKey",
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Accept-Language",
                            },
                        },
                        new string[]
                        {
                        }
                    },
                });

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
        }

        private static void ConfigureLocalization(WebApplicationBuilder builder, IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddDataAnnotationsLocalization();

            builder.Services.AddLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "pt-BR" };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                    new AcceptLanguageHeaderRequestCultureProvider(),
                };
            });
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<ITransientService>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            builder.Services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<IScopedService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            builder.Services.Scan(x =>
                x.FromAssemblies(GetServiceAssemblies())
                .AddClasses(y =>
                    y.AssignableTo<ISingletonService>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }

        private static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityCore<TbUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            var connectionStringsOptions = new ConnectionStringsOptions();
            builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStringsOptions);

            builder.Services.AddDbContext<ApplicationDbContext>(x =>
                x.UseSqlServer(
                    connectionStringsOptions.DefaultConnection,
                    y => y.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }

        private static void ConfigureAutoMapper(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(ApplicationDbContext));
        }

        private static void ConfigureCache(WebApplicationBuilder builder)
        {
            builder.Services.AddDistributedMemoryCache();
        }

        private static void ConfigureJWT(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Jwt));

            var jwtOptions = new JwtOptions();
            builder.Configuration.GetSection(JwtOptions.Jwt).Bind(jwtOptions);

            var validIssuer = jwtOptions.JwtIssuer;
            var validAudience = jwtOptions.JwtAudience;
            var secretKey = jwtOptions.JwtSecurityKey;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        }

        private static void ConfigureCORS(WebApplicationBuilder builder)
        {
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
        }

        private static void ConfigureLogging(WebApplicationBuilder builder)
        {
            builder.Services.AddLogging(x => x.AddDebug());
        }

        private static void ConfigureValidations(WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            builder.Services.AddFluentValidationAutoValidation();
        }

        private static void ConfigureMVC(WebApplicationBuilder builder)
        {
            builder.Services.AddMvc(x =>
            {
                x.Filters.Add(typeof(AuthorizationActionFilter));
                x.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
        }

        private static WebApplication ConfigureMiddlewares(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseRequestLocalization();

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

            return app;
        }

        private static async Task StartDatabase(WebApplication app)
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

        private static IEnumerable<Assembly> GetServiceAssemblies() =>
            [
                Assembly.Load("KomberNet.Infrastructure.DatabaseRepositories"),
                Assembly.Load("KomberNet.Services"),
                Assembly.Load("KomberNet.Services.Billing"),
                Assembly.Load("KomberNet.Services.Financial"),
                Assembly.Load("KomberNet.Services.Inventory"),
                Assembly.Load("KomberNet.Services.Manufacturing"),
                Assembly.Load("KomberNet.Services.Purchasing"),
            ];

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
