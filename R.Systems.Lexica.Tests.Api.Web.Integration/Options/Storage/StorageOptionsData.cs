using R.Systems.Lexica.Infrastructure.Storage.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Storage;

internal class StorageOptionsData : StorageOptions, IOptionsData
{
    public StorageOptionsData()
    {
        DirectoryPath = "/recordings";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(DirectoryPath)}"] = DirectoryPath
        };
    }
}
