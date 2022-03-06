using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Core.Common.Interfaces;

public interface ISetRepository
{
    public Task<OperationResult<Set?>> GetSetAsync(string name);

    public Task<OperationResult<List<Set>?>> GetSetsAsync();
}
