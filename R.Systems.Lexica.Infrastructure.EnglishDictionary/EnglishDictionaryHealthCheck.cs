using Microsoft.Extensions.Diagnostics.HealthChecks;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Queries.GetRecording;

namespace R.Systems.Lexica.Infrastructure.EnglishDictionary;

internal class EnglishDictionaryHealthCheck : IHealthCheck
{
    private readonly IRecordingApi _recordingApi;

    public EnglishDictionaryHealthCheck(IRecordingApi recordingApi)
    {
        _recordingApi = recordingApi;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        try
        {
            await _recordingApi.GetFileAsync("test", WordType.Noun, cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
