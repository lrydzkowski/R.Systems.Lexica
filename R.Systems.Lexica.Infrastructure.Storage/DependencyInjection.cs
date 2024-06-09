using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Storage.Options;
using R.Systems.Lexica.Infrastructure.Storage.Services;

namespace R.Systems.Lexica.Infrastructure.Storage;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureStorageServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptions(configuration, environment);
        services.ConfigureServices();
    }

    private static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.ConfigureOptionsWithValidation<StorageOptions, StorageOptionsValidator>(
            configuration,
            StorageOptions.Position,
            environment
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDirectoryHandler, DirectoryHandler>();
        services.AddSingleton<IFileHandler, FileHandler>();
        services.AddScoped<IRecordingStorage, RecordingStorage>();
    }
}
