using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSet;

public class GetSetQuery : IRequest<GetSetResult>
{
    public List<string> SetPaths { get; init; } = new();
}

public class GetSetResult
{
    public List<Set> Sets { get; init; } = new();
}

public class GetSetQueryHandler : IRequestHandler<GetSetQuery, GetSetResult>
{
    public GetSetQueryHandler(IGetSetRepository getSetRepository)
    {
        GetSetRepository = getSetRepository;
    }

    private IGetSetRepository GetSetRepository { get; }

    public async Task<GetSetResult> Handle(GetSetQuery query, CancellationToken cancellationToken)
    {
        List<Set> sets = new();
        foreach (string setPath in query.SetPaths)
        {
            sets.Add(
                new Set
                {
                    Path = setPath,
                    Entries = await GetSetRepository.GetSetEntriesAsync(setPath)
                }
            );
        }

        return new GetSetResult
        {
            Sets = sets
        };
    }
}
