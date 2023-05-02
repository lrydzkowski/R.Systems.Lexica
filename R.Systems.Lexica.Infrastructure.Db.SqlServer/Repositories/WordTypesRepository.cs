using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Repositories;

internal interface IWordTypesRepository
{
    Task<int?> GetWordTypeIdAsync(WordType wordType, CancellationToken cancellationToken = default);
}

internal class WordTypesRepository : IWordTypesRepository
{
    private readonly AppDbContext _dbContext;

    public WordTypesRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int?> GetWordTypeIdAsync(WordType wordType, CancellationToken cancellationToken = default)
    {
        int? wordTypeId = await _dbContext.WordTypes.Where(x => x.Name == wordType.ToString())
            .Select(x => x.WordTypeId)
            .FirstOrDefaultAsync(cancellationToken);

        return wordTypeId;
    }
}
