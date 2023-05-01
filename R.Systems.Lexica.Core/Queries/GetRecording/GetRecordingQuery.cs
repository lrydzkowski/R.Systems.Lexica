using MediatR;
using R.Systems.Lexica.Core.Common.Domain;

namespace R.Systems.Lexica.Core.Queries.GetRecording;

public class GetRecordingQuery : IRequest<GetRecordingResult>
{
    public string Word { get; set; } = "";

    public WordType WordType { get; set; }
}

public class GetRecordingResult
{
    public byte[]? RecordingFile { get; init; }

    public string? FileName { get; init; }
}

public class GetRecordingQueryHandler : IRequestHandler<GetRecordingQuery, GetRecordingResult>
{
    private readonly IRecordingMetaData _recordingMetaData;
    private readonly IRecordingStorage _recordingStorage;
    private readonly IRecordingApi _recordingApi;

    public GetRecordingQueryHandler(
        IRecordingMetaData recordingMetaData,
        IRecordingStorage recordingStorage,
        IRecordingApi recordingApi
    )
    {
        _recordingMetaData = recordingMetaData;
        _recordingStorage = recordingStorage;
        _recordingApi = recordingApi;
    }

    public async Task<GetRecordingResult> Handle(GetRecordingQuery request, CancellationToken cancellationToken)
    {
        string? fileName = await _recordingMetaData.GetFileNameAsync(request.Word, request.WordType);
        if (fileName != null)
        {
            byte[]? fileFromStorage = await _recordingStorage.GetFileAsync(fileName);
            if (fileFromStorage != null)
            {
                return GetResult(fileFromStorage, request.Word);
            }
        }

        byte[]? fileFromApi = await _recordingApi.GetFileAsync(request.Word, request.WordType);
        if (fileFromApi == null)
        {
            return new GetRecordingResult();
        }

        fileName = Guid.NewGuid().ToString();
        await _recordingStorage.SaveFileAsync(fileName, fileFromApi);
        await _recordingMetaData.AddFileNameAsync(request.Word, request.WordType, fileName);

        return GetResult(fileFromApi, request.Word);
    }

    private GetRecordingResult GetResult(byte[] file, string word)
    {
        return new GetRecordingResult
        {
            RecordingFile = file,
            FileName = GetUserFriendlyFileName(word)
        };
    }

    private string GetUserFriendlyFileName(string word) => $"{word}.mp3";
}
