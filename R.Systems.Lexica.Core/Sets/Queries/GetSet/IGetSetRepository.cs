using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Sets.Queries.GetSet;

public interface IGetSetRepository
{
    public Task<List<Entry>> GetSetEntriesAsync(string filePath);
}
