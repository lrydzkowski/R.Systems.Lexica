using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using R.Systems.Lexica.Core.Commands.CreateSet;
using R.Systems.Lexica.Core.Commands.DeleteSet;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Common.Lists.Extensions;
using R.Systems.Lexica.Core.Queries.GetSet;
using R.Systems.Lexica.Core.Queries.GetSets;
using R.Systems.Lexica.Infrastructure.Db.SqlServer.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.SqlServer.Repositories;

internal class SetsRepository : IGetSetsRepository, IGetSetRepository, IDeleteSetRepository, ICreateSetRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IWordTypesRepository _wordTypesRepository;

    public SetsRepository(AppDbContext dbContext, IWordTypesRepository wordTypesRepository)
    {
        _dbContext = dbContext;
        _wordTypesRepository = wordTypesRepository;
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
            .Select(setEntity => MapToSet(setEntity))
            .FirstOrDefaultAsync(cancellationToken);

        return set;
    }

    public async Task DeleteSetAsync(long setId)
    {
        SetEntity setEntity = await GetSetAsync(setId);

        _dbContext.SetEntities.Remove(setEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateSetAsync(CreateSetCommand createSetCommand)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            SetEntity setEntity = await AddSetAsync(createSetCommand);

            for (int i = 0; i < createSetCommand.Entries.Count; i++)
            {
                Entry entry = createSetCommand.Entries[i];
                int wordTypeId = await GetWordTypeIdAsync(entry);
                WordEntity wordEntity = await AddWordAsync(entry, wordTypeId, i, setEntity);

                for (int j = 0; j < entry.Translations.Count; j++)
                {
                    string translation = entry.Translations[j];
                    await AddTranslationAsync(translation, j, wordEntity);
                }
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> SetNameExists(string setName)
    {
        SetEntity? setEntity = await _dbContext.SetEntities.AsNoTracking()
            .Where(x => x.Name == setName)
            .Select(x => new SetEntity() { SetId = x.SetId })
            .FirstOrDefaultAsync();
        bool exists = setEntity != null;

        return exists;
    }

    private static WordType MapWordType(string wordTypeName)
    {
        bool parsingResult = Enum.TryParse(wordTypeName, out WordType wordType);
        if (!parsingResult)
        {
            return WordType.None;
        }

        return wordType;
    }

    private async Task<SetEntity> GetSetAsync(long setId)
    {
        SetEntity? setEntity = await _dbContext.SetEntities.Where(x => x.SetId == setId)
            .FirstOrDefaultAsync();
        if (setEntity == null)
        {
            throw new ValidationException(
                new List<ValidationFailure>
                {
                    new()
                    {
                        PropertyName = "Set",
                        ErrorMessage = $"Set with the given id doesn't exist ('{setId}').",
                        AttemptedValue = setId,
                        ErrorCode = "NotExist"
                    }
                }
            );
        }

        return setEntity;
    }

    private async Task<SetEntity> AddSetAsync(CreateSetCommand createSetCommand)
    {
        SetEntity setEntity = new()
        {
            Name = createSetCommand.SetName,
            CreatedAtUtc = DateTime.UtcNow
        };
        await _dbContext.SetEntities.AddAsync(setEntity);
        await _dbContext.SaveChangesAsync();

        return setEntity;
    }

    private async Task<int> GetWordTypeIdAsync(Entry entry)
    {
        int? wordTypeId = await _wordTypesRepository.GetWordTypeIdAsync(entry.WordType);
        if (wordTypeId == null)
        {
            throw new InvalidOperationException($"Word type = '{entry.WordType}' doesn't exist.");
        }

        return (int)wordTypeId;
    }

    private async Task<WordEntity> AddWordAsync(Entry entry, int wordTypeId, int order, SetEntity setEntity)
    {
        WordEntity wordEntity = new()
        {
            Word = entry.Word,
            WordTypeId = wordTypeId,
            Order = order,
            SetId = setEntity.SetId
        };
        await _dbContext.WordEntities.AddAsync(wordEntity);
        await _dbContext.SaveChangesAsync();

        return wordEntity;
    }

    private async Task<TranslationEntity> AddTranslationAsync(string translation, int order, WordEntity wordEntity)
    {
        TranslationEntity translationEntity = new()
        {
            Translation = translation,
            Order = order,
            WordId = wordEntity.WordId
        };
        await _dbContext.TranslationEntities.AddAsync(translationEntity);
        await _dbContext.SaveChangesAsync();

        return translationEntity;
    }

    private static Set MapToSet(SetEntity setEntity)
    {
        return new Set
        {
            SetId = setEntity.SetId,
            Name = setEntity.Name,
            CreatedAt = setEntity.CreatedAtUtc,
            Entries = setEntity.Words.OrderBy(x => x.Order)
                .Select(
                    x => new Entry
                    {
                        Word = x.Word,
                        WordType = MapWordType(x.WordType!.Name),
                        Translations = x.Translations.Select(y => y.Translation).ToList()
                    }
                )
                .ToList()
        };
    }
}
