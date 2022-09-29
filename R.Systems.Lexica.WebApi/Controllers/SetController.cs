using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.WebApi.Controllers;

[ApiController]
[Route("sets")]
public class SetController : ControllerBase
{
    public SetController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get sets")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(List<Set>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        GetSetsResult result = await Mediator.Send(new GetSetsQuery());

        return Ok(result.Sets);
    }

    [SwaggerOperation(Summary = "Get the set")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(Set),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404, description: "Set doesn't exist.")]
    [HttpGet, Route("{setName}")]
    public async Task<IActionResult> Get(string setName)
    {
        GetSetResult result = await Mediator.Send(new GetSetQuery { SetName = setName });

        return Ok(result.Set);
    }
}
