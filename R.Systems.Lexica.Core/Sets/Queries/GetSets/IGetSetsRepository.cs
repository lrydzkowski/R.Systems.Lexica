using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSets;

public interface IGetSetsRepository
{
    public Task<List<Set>> GetSetsAsync();
}
