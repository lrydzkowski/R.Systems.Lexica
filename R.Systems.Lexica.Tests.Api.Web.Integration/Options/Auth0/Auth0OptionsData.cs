using R.Systems.Lexica.Infrastructure.Auth0.Options;
using R.Systems.Lexica.Tests.Api.Web.Integration.Common.Options;

namespace R.Systems.Lexica.Tests.Api.Web.Integration.Options.Auth0;

internal class Auth0OptionsData : Auth0Options, IOptionsData
{
    public Auth0OptionsData()
    {
        Domain = "test.eu.auth0.com";
        Audience = "https://test.ddns.net/api/test";
    }

    public Dictionary<string, string?> ConvertToInMemoryCollection(string? parentPosition = null)
    {
        return new Dictionary<string, string?>
        {
            [$"{Position}:{nameof(Domain)}"] = Domain,
            [$"{Position}:{nameof(Audience)}"] = Audience
        };
    }
}
