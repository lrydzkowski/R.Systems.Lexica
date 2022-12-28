using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Forvo.Common.Api;
using R.Systems.Lexica.Infrastructure.Forvo.Common.Options;
using R.Systems.Lexica.Infrastructure.Forvo.Recordings.Queries;

namespace R.Systems.Lexica.Infrastructure.Forvo;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureForvoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ConfigureOptions(services, configuration);
        ConfigureServices(services);
    }

    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<ForvoApiOptions, ForvoApiOptionsValidator>(
            configuration,
            ForvoApiOptions.Position
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IRecordingApi, RecordingApi>();
        services.AddSingleton<IForvoApiClient, ForvoApiClient>();
    }
}
