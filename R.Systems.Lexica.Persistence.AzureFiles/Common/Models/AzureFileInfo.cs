namespace R.Systems.Lexica.Persistence.AzureFiles.Common.Models;

internal class AzureFileInfo
{
    public string FilePath { get; init; } = "";

    public string? Content { get; init; }

    public DateTimeOffset? LastModified { get; init; }
}
