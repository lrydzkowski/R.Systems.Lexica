namespace R.Systems.Lexica.Core.Queries.GetRecording;

public interface IRecordingStorage
{
    Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken);

    Task SaveFileAsync(string fileName, byte[] file);
}
