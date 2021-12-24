using R.Systems.Lexica.Core.Models;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.Core.Interfaces;

public interface ISetRepository
{
    public Task<OperationResult<Set?>> GetSetAsync(string name);

    public Task<OperationResult<List<Set>>> GetSetsAsync();
}
