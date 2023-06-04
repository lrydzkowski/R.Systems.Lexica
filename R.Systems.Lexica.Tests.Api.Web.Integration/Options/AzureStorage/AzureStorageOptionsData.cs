using R.Systems.Lexica.Infrastructure.Azure.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureStorage;

internal class AzureStorageOptionsData : AzureStorageOptions, IOptionsData
{
    public AzureStorageOptionsData()
    {
        BlobData = new AzureStorageBlobOptionsData();
    }

    public AzureStorageBlobOptionsData BlobData { get; set; }

    public new AzureStorageBlobOptions Blob => BlobData;

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return BlobData.ConvertToInMemoryCollection(Position);
    }
}

internal class AzureStorageBlobOptionsData : AzureStorageBlobOptions, IOptionsData
{
    public AzureStorageBlobOptionsData()
    {
        ConnectionString = "connection_string";
        ContainerName = "container_name";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        string position = OptionsPositionsMerger.Merge(parentPosition, Position);

        return new Dictionary<string, string?>
        {
            [$"{position}:{nameof(ConnectionString)}"] = ConnectionString,
            [$"{position}:{nameof(ContainerName)}"] = ContainerName
        };
    }
}
