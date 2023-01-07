using R.Systems.Lexica.Infrastructure.Pronunciation.Common.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.PronunciationApi;

internal class PronunciationApiOptionsData : PronunciationApiOptions, IOptionsData
{
    public PronunciationApiOptionsData()
    {
        ApiUrl = "https://pronunciation.com/[word]";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection()
    {
        return new()
        {
            [$"{Position}:{nameof(ApiUrl)}"] = ApiUrl
        };
    }
}
