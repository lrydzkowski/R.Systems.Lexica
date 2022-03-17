using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Shared.Core.Interfaces;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSets;

public class GetSetsQuery : IDependencyInjectionScoped
{
    public GetSetsQuery(IGetSetsRepository setRepository)
    {
        SetRepository = setRepository;
    }

    public IGetSetsRepository SetRepository { get; }

    public async Task<List<Set>> GetAsync()
    {
        return await SetRepository.GetSetsAsync();
    }
}
