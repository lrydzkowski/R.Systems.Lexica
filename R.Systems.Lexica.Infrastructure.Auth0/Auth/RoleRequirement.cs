using Microsoft.AspNetCore.Authorization;

namespace R.Systems.Lexica.Infrastructure.Auth0.Auth;

internal class RoleRequirement : IAuthorizationRequirement
{
    public RoleRequirement(IReadOnlyCollection<string> roles)
    {
        Roles = roles;
    }

    public IReadOnlyCollection<string> Roles { get; }
}
