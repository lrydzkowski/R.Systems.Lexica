using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSets;

public interface IGetSetsRepository
{
    public Task<List<Set>> GetSetsAsync(ListParameters listParameters);
}
