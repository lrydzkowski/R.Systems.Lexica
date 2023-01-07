using MediatR;

namespace R.Systems.Lexica.Core.Recordings.Queries.GetRecording;

public class GetRecordingQuery : IRequest<GetRecordingResult>
{
    public string Word { get; init; } = "";
}

public class GetRecordingResult
{
    public byte[]? RecordingFile { get; init; }

    public string FileName { get; init; } = "";
}

public class GetRecordingQueryHandler : IRequestHandler<GetRecordingQuery, GetRecordingResult>
{
    public GetRecordingQueryHandler(IRecordingRepository recordingRepository, IRecordingApi recordingApi)
    {
        RecordingRepository = recordingRepository;
        RecordingApi = recordingApi;
    }

    private IRecordingRepository RecordingRepository { get; }
    private IRecordingApi RecordingApi { get; }

    public async Task<GetRecordingResult> Handle(GetRecordingQuery query, CancellationToken cancellationToken)
    {
        string fileName = $"{query.Word}.mp3";
        byte[]? recordingFile = await RecordingRepository.GetRecordingAsync(fileName);
        if (recordingFile == null)
        {
            List<byte[]> newRecordingFiles = await RecordingApi.DownloadRecordingAsync(query.Word);
            if (newRecordingFiles.Count > 0)
            {
                recordingFile = newRecordingFiles[0];
                await RecordingRepository.SaveRecordingAsync(fileName, recordingFile);
            }
        }

        return new GetRecordingResult
        {
            RecordingFile = recordingFile,
            FileName = fileName
        };
    }
}
