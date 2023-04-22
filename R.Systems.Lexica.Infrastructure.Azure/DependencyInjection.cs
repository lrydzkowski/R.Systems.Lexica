using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Azure.Options;

namespace R.Systems.Lexica.Infrastructure.Azure;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureAzureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureAuthentication(configuration);
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<AzureAdOptions, AzureAdOptionsValidator>(
            configuration,
            AzureAdOptions.Position
        );
    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, "AzureAd", AuthenticationSchemes.AzureAd);
    }
}
