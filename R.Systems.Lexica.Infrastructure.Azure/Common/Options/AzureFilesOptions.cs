namespace R.Systems.Lexica.Infrastructure.Azure.Common.Options;

internal class AzureFilesOptions
{
    public const string Position = "AzureFiles";

    public string ConnectionString { get; init; } = "";

    public string FileShareName { get; init; } = "";
}
