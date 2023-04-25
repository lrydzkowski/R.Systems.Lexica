using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetSet;

public class GetSetQuery : IRequest<GetSetResult>
{
    public long SetId { get; init; }
}

public class GetSetResult
{
    public Set? Set { get; init; }
}

public class GetSetQueryHandler : IRequestHandler<GetSetQuery, GetSetResult>
{
    private readonly IGetSetRepository _getSetRepository;

    public GetSetQueryHandler(IGetSetRepository getSetRepository)
    {
        _getSetRepository = getSetRepository;
    }

    public async Task<GetSetResult> Handle(GetSetQuery query, CancellationToken cancellationToken)
    {
        Set? set = await _getSetRepository.GetSetAsync(query.SetId, cancellationToken);

        return new GetSetResult
        {
            Set = set
        };
    }
}
