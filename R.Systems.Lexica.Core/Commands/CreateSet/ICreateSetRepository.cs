namespace R.Systems.Lexica.Core.Commands.CreateSet;

public interface ICreateSetRepository
{
    Task CreateSetAsync(CreateSetCommand createSetCommand);
}
