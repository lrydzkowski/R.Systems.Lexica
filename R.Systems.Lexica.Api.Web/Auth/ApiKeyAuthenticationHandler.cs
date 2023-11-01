using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Api.Web.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace R.Systems.Lexica.Api.Web.Auth;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    private const string ApiKeyName = "api-key";

    private readonly string _apiKey;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<HealthCheckOptions> healthCheckOptions
    ) : base(options, logger, encoder, clock)
    {
        _apiKey = healthCheckOptions.Value.ApiKey;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? apiKey = Request.Headers[ApiKeyName];
        if (apiKey == null)
        {
            apiKey = Request.Query.Where(x => x.Key == ApiKeyName)
                .Select(x => x.Value.FirstOrDefault())
                .FirstOrDefault();
        }

        if (!_apiKey.Equals(apiKey, StringComparison.InvariantCultureIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Wrong API key."));
        }

        ClaimsPrincipal claimsPrincipal = new();
        ClaimsIdentity claimsIdentity = new("APIKey");
        claimsPrincipal.AddIdentity(claimsIdentity);
        AuthenticationTicket authTicket = new(claimsPrincipal, ApiKeyAuthenticationSchemeOptions.Name);

        return Task.FromResult(AuthenticateResult.Success(authTicket));
    }
}
