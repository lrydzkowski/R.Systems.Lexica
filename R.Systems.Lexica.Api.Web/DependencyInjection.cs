﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using R.Systems.Lexica.Api.Web.Auth;
using R.Systems.Lexica.Api.Web.Options;
using R.Systems.Lexica.Api.Web.Services;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Db;
using RunMethodsSequentially;

namespace R.Systems.Lexica.Api.Web;

public static class DependencyInjection
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
        services.ConfigureSwagger();
        services.ConfigureCors();
        services.ConfigureSequentialServices(environment, configuration);
        services.ChangeApiControllerModelValidationResponse();
        services.ConfigureOptions(configuration);
        services.ConfigureAuth();
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "R.Systems.Lexica.Api.Web", Version = "1.0" });
                options.EnableAnnotations();
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    }
                );
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
                );
            }
        );
    }

    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            }
        );
    }

    private static void ConfigureSequentialServices(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        IConfiguration configuration
    )
    {
        services.RegisterRunMethodsSequentially(
                options =>
                {
                    string? connectionString = configuration["ConnectionStrings:AppPostgresDb"]?.Trim();
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        options.AddPostgreSqlLockAndRunMethods(connectionString);
                    }

                    options.AddFileSystemLockAndRunMethods(environment.ContentRootPath);
                }
            )
            .RegisterServiceToRunInJob<AppDbInitializer>();
    }

    private static void ChangeApiControllerModelValidationResponse(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(
            options => options.InvalidModelStateResponseFactory =
                InvalidModelStateService.InvalidModelStateResponseFactory
        );
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<HealthCheckOptions, HealthCheckOptionsValidator>(
            configuration,
            HealthCheckOptions.Position
        );
    }

    private static void ConfigureAuth(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationSchemeOptions.Name,
                null
            );
    }
}
