using MediatR;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSets;

public class GetSetsQuery : GetElementsQuery, IRequest<GetSetsResult>
{
    public bool IncludeSetContent { get; init; } = false;
}

public class GetSetsResult
{
    public List<Set> Sets { get; init; } = new();
}

public class GetSetsQueryHandler : IRequestHandler<GetSetsQuery, GetSetsResult>
{
    public GetSetsQueryHandler(IGetSetsRepository getSetsRepository)
    {
        GetSetsRepository = getSetsRepository;
    }

    private IGetSetsRepository GetSetsRepository { get; }

    public async Task<GetSetsResult> Handle(GetSetsQuery query, CancellationToken cancellationToken)
    {
        return new GetSetsResult
        {
            Sets = await GetSetsRepository.GetSetsAsync(query.ListParameters, query.IncludeSetContent)
        };
    }
}