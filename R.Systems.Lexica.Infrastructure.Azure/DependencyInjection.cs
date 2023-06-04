using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Azure.Options;
using R.Systems.Lexica.Infrastructure.Azure.Services;

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
        services.ConfigureAzureClients();
        services.ConfigureServices();
    }

    private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptionsWithValidation<AzureAdOptions, AzureAdOptionsValidator>(
            configuration,
            AzureAdOptions.Position
        );
        services.ConfigureOptionsWithValidation<AzureStorageOptions, AzureStorageOptionsValidator>(
            configuration,
            AzureStorageOptions.Position
        );
    }

    private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration, "AzureAd", AuthenticationSchemes.AzureAd);
    }

    private static void ConfigureAzureClients(this IServiceCollection services)
    {
        services.AddAzureClients(
            azureClientFactoryBuilder =>
            {
                azureClientFactoryBuilder.AddClient<BlobContainerClient, BlobClientOptions>(
                    (context, serviceProvider) =>
                    {
                        AzureStorageOptions options =
                            serviceProvider.GetRequiredService<IOptions<AzureStorageOptions>>().Value;

                        return new BlobContainerClient(options.Blob.ConnectionString, options.Blob.ContainerName);
                    }
                );
            }
        );
    }

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IRecordingStorage, RecordingStorage>();
    }
}
