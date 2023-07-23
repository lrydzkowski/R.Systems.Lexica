using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using R.Systems.Lexica.Core.Common.Auth;
using R.Systems.Lexica.Infrastructure.Auth0.Options;

namespace R.Systems.Lexica.Infrastructure.Auth0.Auth;

internal class RolesManager
    : IRolesManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Auth0Options _options;

    private IReadOnlyCollection<string>? _roles = null;

    public IReadOnlyCollection<string> Roles
    {
        get
        {
            _roles ??= _httpContextAccessor.HttpContext?.User.Claims
                .Where(claim => claim.Type == _options.RoleClaimName)
                .Select(claim => claim.Value)
                .ToList();

            return _roles ?? new List<string>();
        }
    }


    public RolesManager(IHttpContextAccessor httpContextAccessor, IOptions<Auth0Options> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }
}
