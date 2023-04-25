using MediatR;
using R.Systems.Lexica.Core.Common.Dtos;
using R.Systems.Lexica.Core.Common.Lists;

namespace R.Systems.Lexica.Core.Queries.GetSets;

public class GetSetsQuery : GetElementsQuery, IRequest<GetSetsResult>
{
}

public class GetSetsResult
{
    public ListInfo<SetRecordDto> Sets { get; init; } = new();
}

public class GetSetsQueryHandler : IRequestHandler<GetSetsQuery, GetSetsResult>
{
    private readonly IGetSetsRepository _getSetsRepository;

    public GetSetsQueryHandler(IGetSetsRepository getSetsRepository)
    {
        _getSetsRepository = getSetsRepository;
    }

    public async Task<GetSetsResult> Handle(GetSetsQuery query, CancellationToken cancellationToken)
    {
        ListInfo<SetRecordDto> setRecordsDto =
            await _getSetsRepository.GetSetsAsync(query.ListParameters, cancellationToken);

        return new GetSetsResult()
        {
            Sets = setRecordsDto
        };
    }
}
