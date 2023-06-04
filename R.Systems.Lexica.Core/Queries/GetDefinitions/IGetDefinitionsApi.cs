namespace R.Systems.Lexica.Core.Queries.GetDefinitions;

public interface IGetDefinitionsApi
{
    Task<List<Definition>> GetDefinitionsAsync(string word, CancellationToken cancellationToken);
}
