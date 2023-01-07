using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Pronunciation.Common.Api;
using R.Systems.Lexica.Infrastructure.Pronunciation.Common.Options;
using R.Systems.Lexica.Infrastructure.Pronunciation.Recordings.Queries;

namespace R.Systems.Lexica.Infrastructure.Pronunciation;

public static class DependencyInjection
{
    public static void ConfigureInfrastructurePronunciationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ConfigureOptions(services, configuration);
        ConfigureServices(services);
    }

    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<PronunciationApiOptions, PronunciationApiOptionsValidator>(
            configuration,
            PronunciationApiOptions.Position
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IRecordingApi, RecordingApi>();
        services.AddSingleton<IPronunciationApiClient, PronunciationApiClient>();
    }
}
