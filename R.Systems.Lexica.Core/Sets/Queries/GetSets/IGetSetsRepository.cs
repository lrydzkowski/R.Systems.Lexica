using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSets;

public interface IGetSetsRepository
{
    public Task<ListInfo<Set>> GetSetsAsync(ListParameters listParameters, bool includeSetContent);
}
