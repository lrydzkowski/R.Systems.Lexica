using R.Systems.Lexica.Core.Common.Dtos;
using R.Systems.Lexica.Core.Common.Lists;

namespace R.Systems.Lexica.Core.Queries.GetSets;

public interface IGetSetsRepository
{
    public Task<ListInfo<SetRecordDto>> GetSetsAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    );
}
