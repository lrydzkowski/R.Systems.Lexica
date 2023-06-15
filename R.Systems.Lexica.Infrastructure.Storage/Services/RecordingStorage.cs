using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Storage.Options;

namespace R.Systems.Lexica.Infrastructure.Storage.Services;

internal class RecordingStorage : IRecordingStorage
{
    private readonly StorageOptions _storageOptions;
    private readonly IFileHandler _fileHandler;

    public RecordingStorage(IOptions<StorageOptions> options, IFileHandler fileHandler)
    {
        _storageOptions = options.Value;
        _fileHandler = fileHandler;
    }

    public async Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken)
    {
        string filePath = Path.Combine(_storageOptions.DirectoryPath, fileName);
        if (!_fileHandler.Exists(filePath))
        {
            return null;
        }

        byte[] file = await _fileHandler.ReadAllBytesAsync(filePath, cancellationToken);

        return file;
    }

    public async Task SaveFileAsync(string fileName, byte[] file)
    {
        string filePath = Path.Combine(_storageOptions.DirectoryPath, fileName);

        await _fileHandler.WriteAllBytesAsync(filePath, file);
    }
}
