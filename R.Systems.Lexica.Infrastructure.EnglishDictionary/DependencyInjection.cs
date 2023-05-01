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
        IConfiguration configuration
    )
    {
        services.ConfigureOptions(configuration);
        services.ConfigureServices();
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<EnglishDictionaryOptions, EnglishDictionaryOptionsValidator>(
            configuration,
            EnglishDictionaryOptions.Position
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IApiClient, ApiClient>();
        services.AddSingleton<IRecordingApi, RecordingsService>();
    }
}
