using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSet;

public interface IGetSetRepository
{
    public Task<Set> GetSetAsync(string name);
}
