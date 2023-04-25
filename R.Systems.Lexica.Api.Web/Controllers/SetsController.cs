using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Api.Web.Mappers;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Common.Domain;
using R.Systems.Lexica.Core.Common.Dtos;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Queries.GetSet;
using R.Systems.Lexica.Core.Queries.GetSets;
using R.Systems.Lexica.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[Route("sets")]
public class SetsController : ControllerBase
{
    private readonly ISender _mediator;

    public SetsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get sets")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(ListInfo<SetRecordDto>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet]
    public async Task<IActionResult> GetSets(
        [FromQuery] ListRequest listRequest,
        CancellationToken cancellationToken
    )
    {
        ListMapper mapper = new();
        ListParameters listParameters = mapper.ToListParameter(listRequest);
        GetSetsResult result = await _mediator.Send(
            new GetSetsQuery { ListParameters = listParameters },
            cancellationToken
        );

        return Ok(result.Sets);
    }

    [SwaggerOperation(Summary = "Get set")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(Set),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 404)]
    [SwaggerResponse(statusCode: 500)]
    [HttpGet("{setId}")]
    public async Task<IActionResult> GetSet(
        long setId,
        CancellationToken cancellationToken
    )
    {
        GetSetResult result = await _mediator.Send(
            new GetSetQuery { SetId = setId },
            cancellationToken
        );
        if (result.Set == null)
        {
            return NotFound(null);
        }

        return Ok(result.Set);
    }
}
