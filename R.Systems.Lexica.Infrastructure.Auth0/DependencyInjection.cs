using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Auth0.Options;

namespace R.Systems.Lexica.Infrastructure.Auth0;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureAuth0Services(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAuthentication(configuration);
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<Auth0Options, Auth0OptionsValidator>(
            configuration,
            Auth0Options.Position
        );
    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
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
    }
}
