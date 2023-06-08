using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Common.Domain;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Lexica.Api.Web.Mappers;

[Mapper]
public partial class SetMapper
{
    public partial SetDto ToSetDto(Set set);

    private string MapToWordTypeName(WordType wordType) => WordTypeMapper.MapToWordTypeName(wordType);
}
