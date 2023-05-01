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

    public string FileName { get; init; } = "";
}

public class GetRecordingQueryHandler : IRequestHandler<GetRecordingQuery, GetRecordingResult>
{
    private readonly IGetRecordingService _repository;

    public GetRecordingQueryHandler(IGetRecordingService repository)
    {
        _repository = repository;
    }

    public async Task<GetRecordingResult> Handle(GetRecordingQuery command, CancellationToken cancellationToken)
    {
        byte[]? recordingFile = await _repository.GetRecording(command.Word, command.WordType);
        string fileName = $"{command.Word}.mp3";

        return new GetRecordingResult
        {
            RecordingFile = recordingFile,
            FileName = fileName
        };
    }
}
