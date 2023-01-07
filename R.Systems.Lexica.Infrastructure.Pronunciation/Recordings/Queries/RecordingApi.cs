using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Pronunciation.Common.Api;

namespace R.Systems.Lexica.Infrastructure.Pronunciation.Recordings.Queries;

internal class RecordingApi : IRecordingApi
{
    public RecordingApi(IPronunciationApiClient pronunciationApiClient)
    {
        PronunciationApiClient = pronunciationApiClient;
    }

    private IPronunciationApiClient PronunciationApiClient { get; }

    public async Task<List<byte[]>> DownloadRecordingAsync(string word)
    {
        return await PronunciationApiClient.DownloadPronunciationsAsync(word);
    }
}
