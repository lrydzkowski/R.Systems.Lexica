using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSet;

public class GetSetQuery : IRequest<GetSetResult>
{
    public string SetName { get; init; } = "";
}

public class GetSetResult
{
    public Set Set { get; init; } = new();
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
        return new GetSetResult { Set = await GetSetRepository.GetSetAsync(query.SetName) };
    }
}