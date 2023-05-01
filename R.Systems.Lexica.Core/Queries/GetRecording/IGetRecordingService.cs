using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetRecording;

public interface IGetRecordingService
{
    Task<byte[]?> GetRecording(string word, WordType wordType);
}
