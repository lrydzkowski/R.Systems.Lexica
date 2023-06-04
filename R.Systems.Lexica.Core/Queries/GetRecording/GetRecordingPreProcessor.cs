using MediatR.Pipeline;

namespace R.Systems.Lexica.Core.Queries.GetRecording;

internal class GetRecordingPreProcessor : IRequestPreProcessor<GetRecordingQuery>
{
    public Task Process(GetRecordingQuery request, CancellationToken cancellationToken)
    {
        request.Word = request.Word?.Trim() ?? "";

        return Task.CompletedTask;
    }
}
