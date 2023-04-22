using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Caching;
using Polly.Caching.Memory;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Infrastructure.Wordnik.Common.Options;

namespace R.Systems.Lexica.Infrastructure.Wordnik;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureWordnikServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration)
            .AddMemoryCache()
            .ConfigureServices();
    }

    private static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<WordnikOptions, WordnikOptionsValidator>(
            configuration,
            WordnikOptions.Position
        );

        return services;
    }

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();

        return services;
    }
}
