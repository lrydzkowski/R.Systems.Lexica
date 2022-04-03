using R.Systems.Lexica.Core.Common.Models;

namespace R.Systems.Lexica.Core.Sets.Commands.CreateSet;

public interface ICreateSetRepository
{
    public Task CreateSetAsync(Set set);
}
