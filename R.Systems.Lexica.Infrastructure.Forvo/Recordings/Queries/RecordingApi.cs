using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Forvo.Common.Api;

namespace R.Systems.Lexica.Infrastructure.Forvo.Recordings.Queries;

internal class RecordingApi : IRecordingApi
{
    public RecordingApi(IForvoApiClient forvoApiClient)
    {
        ForvoApiClient = forvoApiClient;
    }

    private IForvoApiClient ForvoApiClient { get; }

    public async Task<List<byte[]>> DownloadRecordingAsync(string word)
    {
        return await ForvoApiClient.DownloadPronunciationsAsync(word);
    }
}
