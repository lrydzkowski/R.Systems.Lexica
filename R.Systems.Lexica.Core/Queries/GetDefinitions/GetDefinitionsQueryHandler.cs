using MediatR;

namespace R.Systems.Lexica.Core.Queries.GetDefinitions;

public class GetDefinitionsQuery : IRequest<GetDefinitionsResult>
{
    public string Word { get; set; } = "";
}

public class GetDefinitionsResult
{
    public List<Definition> Definitions { get; init; } = new();
}

public class GetDefinitionsQueryHandler : IRequestHandler<GetDefinitionsQuery, GetDefinitionsResult>
{
    public GetDefinitionsQueryHandler(IGetDefinitionsApi getDefinitionsApi)
    {
        GetDefinitionsApi = getDefinitionsApi;
    }

    private IGetDefinitionsApi GetDefinitionsApi { get; }

    public async Task<GetDefinitionsResult> Handle(GetDefinitionsQuery query, CancellationToken cancellationToken)
    {
        List<Definition> definitions = await GetDefinitionsApi.GetDefinitionsAsync(query.Word, cancellationToken);

        return new GetDefinitionsResult
        {
            Definitions = definitions
        };
    }
}
