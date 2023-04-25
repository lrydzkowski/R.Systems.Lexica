using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetSet;

public interface IGetSetRepository
{
    Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken);
}
