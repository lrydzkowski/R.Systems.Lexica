using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Common.Errors;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
[SwaggerResponse(
    StatusCodes.Status422UnprocessableEntity,
    type: typeof(IEnumerable<ErrorInfo>),
    contentTypes: [MediaTypeNames.Application.Json]
)]
[SwaggerResponse(StatusCodes.Status500InternalServerError)]
public abstract class ApiControllerBase : ControllerBase
{
}
