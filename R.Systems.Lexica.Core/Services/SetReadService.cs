using R.Systems.Lexica.Core.Interfaces;
using R.Systems.Lexica.Core.Models;
using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Core.Services;

public class SetReadService : IDependencyInjectionScoped
{
    public SetReadService(ISetRepository setRepository)
    {
        SetRepository = setRepository;
    }

    public ISetRepository SetRepository { get; }

    public async Task<OperationResult<Set?>> GetAsync(string name)
    {
        return await SetRepository.GetSetAsync(name);
    }

    public async Task<OperationResult<List<Set>?>> GetAsync()
    {
        return await SetRepository.GetSetsAsync();
    }
}
