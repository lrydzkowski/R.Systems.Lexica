using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Dtos;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Common.Lists.Extensions;
using R.Systems.Lexica.Core.Queries.GetSet;
using R.Systems.Lexica.Core.Queries.GetSets;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Repositories;

internal class SetsRepository : IGetSetsRepository, IGetSetRepository
{
    private readonly AppDbContext _dbContext;

    public SetsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ListInfo<SetRecordDto>> GetSetsAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        List<string> fieldsAvailableToSort = new() { "setId", "name", "createdAtUtc" };
        List<string> fieldsAvailableToFilter = new() { "setId", "name" };

        List<SetRecordDto> sets = await _dbContext.SetEntities.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "setId")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Paginate(listParameters.Pagination)
            .Select(
                setEntity => new SetRecordDto
                {
                    SetId = setEntity.SetId,
                    Name = setEntity.Name,
                    CreatedAt = setEntity.CreatedAtUtc
                }
            )
            .ToListAsync(cancellationToken);
        int count = await _dbContext.SetEntities.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "setId")
            .Filter(fieldsAvailableToFilter, listParameters.Search)
            .Select(
                setEntity => (int)setEntity.SetId!
            )
            .CountAsync(cancellationToken);

        return new ListInfo<SetRecordDto>
        {
            Data = sets,
            Count = count
        };
    }

    public async Task<Set?> GetSetAsync(long setId, CancellationToken cancellationToken)
    {
        Set? set = await _dbContext.SetEntities.AsNoTracking()
            .Where(setEntity => setEntity.SetId == setId)
            .Select(
                setEntity => new Set
                {
                    SetId = setEntity.SetId,
                    Name = setEntity.Name,
                    CreatedAt = setEntity.CreatedAtUtc,
                    Entries = setEntity.SetWords.OrderBy(x => x.Order)
                        .Select(
                            x => new Entry
                            {
                                Word = x.Word!.Word, WordType = MapWordType(x.Word!.WordType!.Name),
                                Translations = x.Word!.Translations.Select(y => y.Translation).ToList()
                            }
                        )
                        .ToList()
                }
            )
            .FirstOrDefaultAsync(cancellationToken);

        return set;
    }

    private static WordTypes MapWordType(string wordTypeName)
    {
        bool parsingResult = Enum.TryParse(wordTypeName, out WordTypes wordType);
        if (!parsingResult)
        {
            return WordTypes.None;
        }

        return wordType;
    }
}
