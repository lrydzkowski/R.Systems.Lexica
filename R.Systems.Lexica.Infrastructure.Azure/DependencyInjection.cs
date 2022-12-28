using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Infrastructure.Azure.Common.FileShare;
using R.Systems.Lexica.Infrastructure.Azure.Common.Options;
using R.Systems.Lexica.Infrastructure.Azure.Recordings.Queries;
using R.Systems.Lexica.Infrastructure.Azure.Sets.Common;
using R.Systems.Lexica.Infrastructure.Azure.Sets.Queries;

namespace R.Systems.Lexica.Infrastructure.Azure;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureAzureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        ConfigureOptions(services, configuration);
        ConfigureMicrosoftIdentity(services, configuration);
        ConfigureAzureClients(services);
        ConfigureServices(services);
    }

    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<AzureFilesOptions, AzureFilesOptionsValidator>(
            configuration,
            AzureFilesOptions.Position
        );
        services.ConfigureOptionsWithValidation<AzureAdOptions, AzureAdOptionsValidator>(
            configuration,
            AzureAdOptions.Position
        );
    }

    private static void ConfigureMicrosoftIdentity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMicrosoftIdentityWebApiAuthentication(configuration);
    }

    private static void ConfigureAzureClients(IServiceCollection services)
    {
        services.AddAzureClients(
            azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddClient<ShareClient, ShareClientOptions>(
                    (context, serviceProvider) =>
                    {
                        AzureFilesOptions options =
                            serviceProvider.GetRequiredService<IOptions<AzureFilesOptions>>().Value;

                        return new ShareClient(options.ConnectionString, options.FileShareName);
                    }
                );
            }
        );
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IFileShareClient, FileShareClient>();
        services.AddScoped<IGetSetsRepository, GetSetsRepository>();
        services.AddScoped<IGetSetRepository, GetSetRepository>();
        services.AddScoped<SetParser>();
        services.AddScoped<IRecordingRepository, RecordingRepository>();
    }
}
