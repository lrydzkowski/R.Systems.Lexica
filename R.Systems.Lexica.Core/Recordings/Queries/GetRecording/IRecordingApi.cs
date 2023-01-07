namespace R.Systems.Lexica.Core.Recordings.Queries.GetRecording;

public interface IRecordingApi
{
    Task<List<byte[]>> DownloadRecordingAsync(string word);
}
