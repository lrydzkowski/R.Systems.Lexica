using MediatR;
using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSets;

public class GetSetsQuery : IRequest<List<Set>>
{
}

public class GetSetsQueryHandler : IRequestHandler<GetSetsQuery, List<Set>>
{
    public GetSetsQueryHandler(IGetSetsRepository repository)
    {
        Repository = repository;
    }

    public IGetSetsRepository Repository { get; }

    public async Task<List<Set>> Handle(GetSetsQuery request, CancellationToken cancellationToken)
    {
        return await Repository.GetSetsAsync();
    }
}
