using R.Systems.Lexica.Core.Common.Interfaces;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Shared.Core.Interfaces;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Core.Common.Services;

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
