namespace R.Systems.Lexica.Core.Commands.UpdateSet;

public interface IUpdateSetRepository
{
    Task UpdateSetAsync(UpdateSetCommand updateSetCommand);
}
