namespace R.Systems.Lexica.Infrastructure.Azure.Common.Options;

internal class AzureFilesOptions
{
    public const string Position = "AzureFiles";

    public string ConnectionString { get; init; } = "";

    public string FileShareName { get; init; } = "";

    public string SetsDirectoryPath { get; init; } = "";

    public string RecordingsDirectoryPath { get; init; } = "";
}
