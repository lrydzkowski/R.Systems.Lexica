using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Api.Web.Mappers;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Queries.GetAppInfo;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
public class AppController : ControllerBase
{
    public AppController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get basic information about application")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(GetAppInfoResponse),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet, Route("")]
    public async Task<IActionResult> GetAppInfo()
    {
        GetAppInfoResult result = await Mediator.Send(
            new GetAppInfoQuery { AppAssembly = Assembly.GetExecutingAssembly() }
        );
        GetAppInfoMapper mapper = new();
        GetAppInfoResponse response = mapper.ToResponse(result);

        return Ok(response);
    }
}
