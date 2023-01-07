using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
[Route("sets")]
public class SetsController : ControllerBase
{
    public SetsController(IMapper mapper, ISender mediator)
    {
        Mapper = mapper;
        Mediator = mediator;
    }

    private IMapper Mapper { get; }
    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get sets")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(ListInfo<Set>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [Authorize, RequiredScope("Access")]
    [HttpGet]
    public async Task<IActionResult> GetSets(
        [FromQuery] ListRequest listRequest,
        [FromQuery] bool includeSetContent = false
    )
    {
        ListParameters listParameters = Mapper.Map<ListParameters>(listRequest);
        GetSetsResult result = await Mediator.Send(
            new GetSetsQuery { ListParameters = listParameters, IncludeSetContent = includeSetContent }
        );

        return Ok(result.Sets);
    }

    [SwaggerOperation(Summary = "Get set")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(ListInfo<Set>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [Authorize, RequiredScope("Access")]
    [Route("{setPaths}")]
    [HttpGet]
    public async Task<IActionResult> GetSet(string setPaths)
    {
        GetSetResult result = await Mediator.Send(
            new GetSetQuery { SetPaths = WebUtility.UrlDecode(setPaths).Split("|").ToList() }
        );

        return Ok(result.Sets);
    }
}
