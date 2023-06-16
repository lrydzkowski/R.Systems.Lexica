using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Storage.Options;

namespace R.Systems.Lexica.Infrastructure.Storage.Services;

internal class RecordingStorage : IRecordingStorage
{
    private readonly StorageOptions _storageOptions;
    private readonly IFileHandler _fileHandler;
    private readonly IDirectoryHandler _directoryHandler;

    public RecordingStorage(
        IOptions<StorageOptions> options,
        IFileHandler fileHandler,
        IDirectoryHandler directoryHandler
    )
    {
        _storageOptions = options.Value;
        _fileHandler = fileHandler;
        _directoryHandler = directoryHandler;
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
        if (!_directoryHandler.Exists(_storageOptions.DirectoryPath))
        {
            _directoryHandler.CreateDirectory(_storageOptions.DirectoryPath);
        }

        string filePath = Path.Combine(_storageOptions.DirectoryPath, fileName);

        await _fileHandler.WriteAllBytesAsync(filePath, file);
    }
}
