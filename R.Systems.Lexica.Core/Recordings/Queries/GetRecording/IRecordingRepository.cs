namespace R.Systems.Lexica.Core.Recordings.Queries.GetRecording;

public interface IRecordingRepository
{
    Task<byte[]?> GetRecordingAsync(string fileName);

    Task SaveRecordingAsync(string fileName, byte[] recordingFile);
}
