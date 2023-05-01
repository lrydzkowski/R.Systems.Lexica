using Azure.Storage.Blobs;
using R.Systems.Lexica.Core.Queries.GetRecording;

namespace R.Systems.Lexica.Infrastructure.Azure.Services;

internal class RecordingsService : IRecordingStorage
{
    private readonly BlobContainerClient _blobContainerClient;

    public RecordingsService(BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
    }

    public async Task<byte[]?> GetFileAsync(string fileName)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

        using MemoryStream stream = new();
        await blobClient.DownloadToAsync(stream);
        byte[] file = stream.ToArray();

        return file;
    }

    public async Task SaveFileAsync(string fileName, byte[] file)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        BinaryData binaryData = new(file);
        await blobClient.UploadAsync(binaryData, true);
    }
}
