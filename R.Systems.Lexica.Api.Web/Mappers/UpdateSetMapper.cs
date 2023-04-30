using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Commands.UpdateSet;
using R.Systems.Lexica.Core.Common.Domain;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Lexica.Api.Web.Mappers;

[Mapper]
public partial class UpdateSetMapper
{
    public partial UpdateSetCommand ToCommand(UpdateSetRequest request);

    private WordType MapToWordType(string wordType)
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
