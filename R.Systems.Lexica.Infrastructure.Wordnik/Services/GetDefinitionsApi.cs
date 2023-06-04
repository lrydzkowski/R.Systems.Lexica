using R.Systems.Lexica.Core.Queries.GetDefinitions;
using R.Systems.Lexica.Infrastructure.Wordnik.Models;

namespace R.Systems.Lexica.Infrastructure.Wordnik.Services;

internal class GetDefinitionsApi : IGetDefinitionsApi
{
    private readonly WordApi _wordApi;

    public GetDefinitionsApi(WordApi wordApi)
    {
        _wordApi = wordApi;
    }

    public async Task<List<Definition>> GetDefinitionsAsync(string word, CancellationToken cancellationToken)
    {
        DefinitionDtoMapper mapper = new();
        List<DefinitionDto> definitionsDto = await _wordApi.GetDefinitionsAsync(word, cancellationToken);

        return mapper.ToDefinitions(definitionsDto);
    }
}
