using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Api.Web.Mappers;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Commands.CreateSet;
using R.Systems.Lexica.Core.Commands.DeleteSet;
using R.Systems.Lexica.Core.Commands.UpdateSet;
using R.Systems.Lexica.Core.Common.Lists;
using R.Systems.Lexica.Core.Queries.GetSet;
using R.Systems.Lexica.Core.Queries.GetSets;
using R.Systems.Lexica.Infrastructure.Auth0.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[Route("sets")]
public class SetsController : ApiControllerWithAuthBase
{
    private readonly ISender _mediator;

    public SetsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get sets")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(ListInfo<SetRecordDto>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
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
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(SetDto),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [SwaggerResponse(StatusCodes.Status404NotFound)]
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

        SetMapper mapper = new();
        SetDto setDto = mapper.ToSetDto(result.Set);


        return Ok(setDto);
    }

    [SwaggerOperation(Summary = "Delete set")]
    [SwaggerResponse(
        StatusCodes.Status204NoContent,
        description: "Set deleted"
    )]
    [Authorize(Policy = AuthorizationPolicies.IsAdmin)]
    [HttpDelete("{setId}")]
    public async Task<IActionResult> DeleteSet(
        long setId,
        CancellationToken cancellationToken
    )
    {
        await _mediator.Send(
            new DeleteSetCommand { SetId = setId },
            cancellationToken
        );

        return NoContent();
    }

    [SwaggerOperation(Summary = "Create set")]
    [SwaggerResponse(
        StatusCodes.Status201Created,
        description: "Set created",
        type: typeof(CreateSetResult)
    )]
    [Authorize(Policy = AuthorizationPolicies.IsAdmin)]
    [HttpPost]
    public async Task<IActionResult> CreateSet(CreateSetRequest createSetRequest)
    {
        CreateSetMapper mapper = new();
        CreateSetCommand command = mapper.ToCommand(createSetRequest);
        CreateSetResult result = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetSet),
            new { setId = result.SetId },
            result
        );
    }

    [SwaggerOperation(Summary = "Update set")]
    [SwaggerResponse(
        StatusCodes.Status204NoContent,
        description: "Set updated"
    )]
    [Authorize(Policy = AuthorizationPolicies.IsAdmin)]
    [HttpPut]
    public async Task<IActionResult> UpdateSet(UpdateSetRequest updateSetRequest)
    {
        UpdateSetMapper mapper = new();
        UpdateSetCommand command = mapper.ToCommand(updateSetRequest);
        await _mediator.Send(command);

        return NoContent();
    }
}
