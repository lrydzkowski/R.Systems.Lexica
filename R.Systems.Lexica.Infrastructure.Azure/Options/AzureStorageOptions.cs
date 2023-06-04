namespace R.Systems.Lexica.Infrastructure.Azure.Options;

internal class AzureStorageOptions
{
    public const string Position = "AzureStorage";

    public AzureStorageBlobOptions Blob { get; init; } = new();
}

internal class AzureStorageBlobOptions
{
    public const string Position = "Blob";

    public string ConnectionString { get; init; } = "";

    public string ContainerName { get; init; } = "";
}
