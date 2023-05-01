using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetRecording;

public interface IRecordingApi
{
    Task<byte[]?> GetFileAsync(string word, WordType wordType);
}
