using Microsoft.AspNetCore.Authentication;

namespace R.Systems.Lexica.Api.Web.Auth;

public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public const string Name = "ApiKeyAuthenticationScheme";
}
