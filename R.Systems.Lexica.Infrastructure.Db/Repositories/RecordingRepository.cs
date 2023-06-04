using Microsoft.EntityFrameworkCore;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Db.Common.Entities;

namespace R.Systems.Lexica.Infrastructure.Db.Repositories;

internal class RecordingRepository : IRecordingMetaData
{
    private readonly AppDbContext _dbContext;
    private readonly IWordTypesRepository _wordTypesRepository;

    public RecordingRepository(AppDbContext dbContext, IWordTypesRepository wordTypesRepository)
    {
        _dbContext = dbContext;
        _wordTypesRepository = wordTypesRepository;
    }

    public async Task<string?> GetFileNameAsync(string word, WordType wordType, CancellationToken cancellationToken)
    {
        int wordTypeId = await GetWordTypeIdAsync(wordType, cancellationToken);
        string? fileName = await _dbContext.RecordingEntities.AsNoTracking()
            .Where(x => x.Word == word && x.WordTypeId == wordTypeId)
            .Select(x => x.FileName)
            .FirstOrDefaultAsync(cancellationToken);

        return fileName;
    }

    public async Task AddFileNameAsync(string word, WordType wordType, string fileName)
    {
        bool isNew = false;
        RecordingEntity? recordingEntity = await _dbContext.RecordingEntities.FirstOrDefaultAsync(x => x.Word == word);
        if (recordingEntity == null)
        {
            isNew = true;
            recordingEntity = new RecordingEntity();
        }

        recordingEntity.Word = word;
        recordingEntity.WordTypeId = await GetWordTypeIdAsync(wordType);
        recordingEntity.FileName = fileName;
        if (isNew)
        {
            await _dbContext.RecordingEntities.AddAsync(recordingEntity);
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task<int> GetWordTypeIdAsync(WordType wordType, CancellationToken cancellationToken = default)
    {
        int? wordTypeId = await _wordTypesRepository.GetWordTypeIdAsync(wordType, cancellationToken);
        if (wordTypeId == null)
        {
            throw new InvalidOperationException($"Word type '{wordType}' doesn't exist in database.");
        }

        return (int)wordTypeId;
    }
}
