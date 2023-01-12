using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace R.Systems.Lexica.Api.Web;

public static class DependencyInjection
{
    public static void ConfigureServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();
        services.ConfigureCors();
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddApplicationInsightsTelemetry();
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "R.Systems.Lexica.Api.Web", Version = "1.0" });
                options.EnableAnnotations();
                OpenApiSecurityScheme jwtSecurityScheme = new()
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        { jwtSecurityScheme, Array.Empty<string>() }
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
}
