using Microsoft.AspNetCore.Authorization;
using R.Systems.Lexica.Infrastructure.Auth0;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[Authorize(AuthenticationSchemes = AuthenticationSchemes.Auth0)]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
[SwaggerResponse(StatusCodes.Status403Forbidden)]
public class ApiControllerWithAuthBase : ApiControllerBase
{
}
