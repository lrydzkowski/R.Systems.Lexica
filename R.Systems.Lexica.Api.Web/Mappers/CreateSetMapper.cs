using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Commands.CreateSet;
using R.Systems.Lexica.Core.Common.Domain;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Lexica.Api.Web.Mappers;

[Mapper]
public partial class CreateSetMapper
{
    public partial CreateSetCommand ToCommand(CreateSetRequest request);

    private WordType MapToWordType(string wordType) => WordTypeMapper.MapToWordType(wordType);
}
