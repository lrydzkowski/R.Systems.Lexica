using R.Systems.Lexica.Core.Queries.GetDefinitions;
using Riok.Mapperly.Abstractions;

namespace R.Systems.Lexica.Infrastructure.Wordnik.Models;

[Mapper]
internal partial class DefinitionDtoMapper
{
    public partial List<Definition> ToDefinitions(List<DefinitionDto> definitionDtos);

    private string MapToString(DefinitionExampleUsesDto exampleUsesDto)
    {
        return exampleUsesDto.Text;
    }
}
