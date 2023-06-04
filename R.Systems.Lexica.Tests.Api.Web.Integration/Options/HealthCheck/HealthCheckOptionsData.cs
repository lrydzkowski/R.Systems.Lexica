using R.Systems.Lexica.Api.Web.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.HealthCheck;

internal class HealthCheckOptionsData : HealthCheckOptions, IOptionsData
{
    public HealthCheckOptionsData()
    {
        ApiKey = "0564FD25-B9C9-43F3-AA9A-F11082ADD4E7";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(ApiKey)}"] = ApiKey
        };
    }
}
