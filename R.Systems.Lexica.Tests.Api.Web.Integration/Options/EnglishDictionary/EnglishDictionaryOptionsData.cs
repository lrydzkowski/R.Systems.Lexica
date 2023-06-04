using R.Systems.Lexica.Infrastructure.EnglishDictionary.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.EnglishDictionary;

internal class EnglishDictionaryOptionsData : EnglishDictionaryOptions, IOptionsData
{
    public EnglishDictionaryOptionsData()
    {
        Host = "host";
        Path = "/dictionary/english/{word}";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Host)}"] = Host,
            [$"{Position}:{nameof(Path)}"] = Path
        };
    }
}
