using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetRecording;

public interface IRecordingMetaData
{
    Task<string?> GetFileNameAsync(string word, WordType wordType);

    Task AddFileNameAsync(string word, WordType wordType, string fileName);
}
