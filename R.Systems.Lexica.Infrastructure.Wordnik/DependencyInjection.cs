using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Queries.GetDefinitions;
using R.Systems.Lexica.Infrastructure.Wordnik.Options;
using R.Systems.Lexica.Infrastructure.Wordnik.Services;

namespace R.Systems.Lexica.Infrastructure.Wordnik;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureWordnikServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptions(configuration, environment)
            .AddMemoryCache()
            .ConfigureServices()
            .ConfigureHealthChecks();
    }

    private static IServiceCollection ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptionsWithValidation<WordnikOptions, WordnikOptionsValidator>(
            configuration,
            WordnikOptions.Position,
            environment
        );

        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<WordApi>()
            .AddSingleton<IGetDefinitionsApi, GetDefinitionsApi>();

        return services;
    }

    private static void ConfigureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<WordnikHealthCheck>(nameof(WordnikHealthCheck));
    }
}
