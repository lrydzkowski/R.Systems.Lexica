namespace R.Systems.Lexica.Persistence.AzureFiles.Common.Options;

internal class AzureFilesOptions
{
    public const string Position = "AzureFiles";

    public string ConnectionString { get; init; } = "";

    public string FileShareName { get; init; } = "";
}
