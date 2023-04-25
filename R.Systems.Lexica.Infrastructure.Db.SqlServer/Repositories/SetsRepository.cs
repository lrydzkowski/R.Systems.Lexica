using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Core.Common.Dtos;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Common.Lists.Extensions;
using R.Systems.Lexica.Core.Queries.GetSets;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Repositories;

internal class SetsRepository : IGetSetsRepository
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
        List<string> fieldsAvailableToSort = new() { "set_id", "name", "created_at_utc" };
        List<string> fieldsAvailableToFilter = new() { "set_id", "name" };

        List<SetRecordDto> sets = await _dbContext.SetEntities.AsNoTracking()
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "set_id")
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
            .Sort(fieldsAvailableToSort, listParameters.Sorting, "set_id")
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
}
