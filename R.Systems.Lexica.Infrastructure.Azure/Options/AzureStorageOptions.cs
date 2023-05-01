namespace R.Systems.Lexica.Infrastructure.Azure.Options;

public class AzureStorageOptions
{
    public const string Position = "AzureStorage";

    public BlobOptions Blob { get; init; } = new();
}

public class BlobOptions
{
    public const string Position = "Blob";

    public string ConnectionString { get; init; } = "";

    public string ContainerName { get; init; } = "";
}
