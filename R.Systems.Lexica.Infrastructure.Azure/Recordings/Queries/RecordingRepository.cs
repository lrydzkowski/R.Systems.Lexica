using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Azure.Common.FileShare;
using R.Systems.Lexica.Infrastructure.Azure.Common.Options;

namespace R.Systems.Lexica.Infrastructure.Azure.Recordings.Queries;

internal class RecordingRepository : IRecordingRepository
{
    public RecordingRepository(IOptions<AzureFilesOptions> options, IFileShareClient fileShareClient)
    {
        FileShareClient = fileShareClient;
        Options = options.Value;
    }

    private AzureFilesOptions Options { get; }
    private IFileShareClient FileShareClient { get; }

    public async Task<byte[]?> GetRecordingAsync(string fileName)
    {
        return await FileShareClient.GetFileAsync(fileName, Options.RecordingsDirectoryPath);
    }

    public async Task SaveRecordingAsync(string fileName, byte[] recordingFile)
    {
        await FileShareClient.SaveFileAsync(recordingFile, fileName, Options.RecordingsDirectoryPath);
    }
}
