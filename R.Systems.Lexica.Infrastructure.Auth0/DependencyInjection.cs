using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Common.Auth;
using R.Systems.Lexica.Infrastructure.Auth0.Auth;
using R.Systems.Lexica.Infrastructure.Auth0.Options;

namespace R.Systems.Lexica.Infrastructure.Auth0;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureAuth0Services(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptions(configuration, environment);
        services.ConfigureAuth(configuration);
    }

    private static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptionsWithValidation<Auth0Options, Auth0OptionsValidator>(
            configuration,
            Auth0Options.Position,
            environment
        );
    }

    private static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication()
            .AddJwtBearer(
                AuthenticationSchemes.Auth0,
                options =>
                {
                    options.Authority = $"https://{configuration["Auth0:Domain"]}/";
                    options.Audience = configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                }
            );
        services.AddSingleton<IAuthorizationHandler, RoleHandler>();
        services.AddAuthorization(
            options =>
            {
                options.AddPolicy(
                    AuthorizationPolicies.IsAdmin,
                    policy => policy.RequireAuthenticatedUser()
                        .AddRequirements(new RoleRequirement(new List<string> { "admin" }))
                );
            }
        );
        services.AddScoped<IRolesManager, RolesManager>();
    }
}
