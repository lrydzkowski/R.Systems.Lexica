using Microsoft.Extensions.Diagnostics.HealthChecks;
using R.Systems.Lexica.Infrastructure.Wordnik.Services;

namespace R.Systems.Lexica.Infrastructure.Wordnik;

internal class WordnikHealthCheck : IHealthCheck
{
    private readonly WordApi _wordApi;

    public WordnikHealthCheck(WordApi wordApi)
    {
        _wordApi = wordApi;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        try
        {
            await _wordApi.GetDefinitionsAsync("test", cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
