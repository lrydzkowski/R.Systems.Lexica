using R.Systems.Lexica.Infrastructure.Azure.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.AzureFiles;

internal class AzureFilesOptionsData : AzureFilesOptions, IOptionsData
{
    public AzureFilesOptionsData()
    {
        ConnectionString =
            "DefaultEndpointsProtocol=https;AccountName=test;AccountKey=test;EndpointSuffix=core.windows.net";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new()
        {
            [$"{Position}:{nameof(ConnectionString)}"] = ConnectionString
        };
    }
}
