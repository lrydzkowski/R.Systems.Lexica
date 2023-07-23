using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Infrastructure.Auth0.Options;

namespace R.Systems.Lexica.Infrastructure.Auth0.Auth;

internal class RoleHandler : AuthorizationHandler<RoleRequirement>
{
    private readonly Auth0Options _auth0Options;

    public RoleHandler(IOptions<Auth0Options> options)
    {
        _auth0Options = options.Value;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        IReadOnlyCollection<string> roles = context.User.Claims
            .Where(claim => claim.Type == _auth0Options.RoleClaimName)
            .Select(claim => claim.Value)
            .ToList();
        if (roles.Count == 0)
        {
            return Fail(context);
        }

        if (!ContainsRole(roles, requirement.Roles))
        {
            return Fail(context);
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }

    private Task Fail(AuthorizationHandlerContext context)
    {
        context.Fail();

        return Task.CompletedTask;
    }

    private bool ContainsRole(IReadOnlyCollection<string> roles, IReadOnlyCollection<string> requiredRoles)
    {
        return roles.Any(role => requiredRoles.Any(requiredRole => requiredRole == role));
    }
}
