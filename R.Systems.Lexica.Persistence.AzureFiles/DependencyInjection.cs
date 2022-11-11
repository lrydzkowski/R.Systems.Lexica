using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Lexica.Persistence.AzureFiles.Common.FileShare;
using R.Systems.Lexica.Persistence.AzureFiles.Common.Options;
using R.Systems.Lexica.Persistence.AzureFiles.Sets.Queries.GetSet;
using R.Systems.Lexica.Persistence.AzureFiles.Sets.Queries.GetSets;

namespace R.Systems.Lexica.Persistence.AzureFiles;

public static class DependencyInjection
{
    public static void ConfigurePersistenceAzureFilesServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.ConfigureOptionsWithValidation<AzureFilesOptions, AzureFilesOptionsValidator>(
            configuration,
            AzureFilesOptions.Position
        );
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
        services.AddScoped<IFileShareClient, FileShareClient>();
        services.AddScoped<IGetSetsRepository, GetSetsRepository>();
        services.AddScoped<IGetSetRepository, GetSetRepository>();
    }
}
