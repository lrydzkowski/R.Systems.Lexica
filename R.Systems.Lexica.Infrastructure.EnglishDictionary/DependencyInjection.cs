using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.EnglishDictionary.Options;
using R.Systems.Lexica.Infrastructure.EnglishDictionary.Services;

namespace R.Systems.Lexica.Infrastructure.EnglishDictionary;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureEnglishDictionaryServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptions(configuration, environment);
        services.ConfigureServices();
        services.ConfigureHealthChecks();
    }

    private static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptionsWithValidation<EnglishDictionaryOptions, EnglishDictionaryOptionsValidator>(
            configuration,
            EnglishDictionaryOptions.Position,
            environment
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IApiClient, ApiClient>();
        services.AddSingleton<IRecordingApi, RecordingService>();
    }

    private static void ConfigureHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<EnglishDictionaryHealthCheck>(nameof(EnglishDictionaryHealthCheck));
    }
}
