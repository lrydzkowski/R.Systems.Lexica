using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Shared.Core.Interfaces;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSet;

public class GetSetQuery : IDependencyInjectionScoped
{
    public GetSetQuery(IGetSetRepository setRepository)
    {
        SetRepository = setRepository;
    }

    public IGetSetRepository SetRepository { get; }

    public async Task<Set> GetAsync(string name)
    {
        return await SetRepository.GetSetAsync(name);
    }
}
