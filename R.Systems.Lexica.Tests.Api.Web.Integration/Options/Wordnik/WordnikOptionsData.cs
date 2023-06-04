using R.Systems.Lexica.Infrastructure.Wordnik.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Wordnik;

internal class WordnikOptionsData : WordnikOptions, IOptionsData
{
    public WordnikOptionsData()
    {
        ApiBaseUrl = "https://test.com";
        DefinitionsUrl = "/words/{word}/definitions";
        ApiKey = "rgerg34tg34gbrrfb";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(ApiBaseUrl)}"] = ApiBaseUrl,
            [$"{Position}:{nameof(DefinitionsUrl)}"] = DefinitionsUrl,
            [$"{Position}:{nameof(ApiKey)}"] = ApiKey
        };
    }
}
