namespace R.Systems.Lexica.Core.Commands.DeleteSet;

public interface IDeleteSetRepository
{
    Task DeleteSetAsync(long setId);
}
