using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Api.Web.Mappers;

public static class WordTypeMapper
{
    public static WordType MapToWordType(string wordType)
    {
        return wordType switch
        {
            "noun" => WordType.Noun,
            "verb" => WordType.Verb,
            "adjective" => WordType.Adjective,
            "adverb" => WordType.Adverb,
            _ => WordType.None
        };
    }
}
