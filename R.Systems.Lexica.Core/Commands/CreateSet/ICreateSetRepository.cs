namespace R.Systems.Lexica.Core.Commands.CreateSet;

public interface ICreateSetRepository
{
    Task<long> CreateSetAsync(CreateSetCommand createSetCommand);

    Task<bool> SetExistsAsync(string setName, CancellationToken cancellationToken);
}
