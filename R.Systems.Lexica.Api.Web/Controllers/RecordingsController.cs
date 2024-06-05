using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Api.Web.Mappers;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Queries.GetRecording;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[Route("recordings")]
public class RecordingsController : ApiControllerWithAuthBase
{
    private readonly ISender _mediator;

    public RecordingsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get recording")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(byte[]),
        contentTypes: ["audio/mpeg"]
    )]
    [HttpGet("{word}")]
    public async Task<IActionResult> GetRecording(
        [FromQuery] GetRecordingRequest request,
        CancellationToken cancellationToken
    )
    {
        GetRecordingMapper mapper = new();
        GetRecordingQuery query = mapper.MapToCommand(request);
        GetRecordingResult result = await _mediator.Send(query, cancellationToken);
        if (result.RecordingFile == null)
        {
            return NotFound(null);
        }

        return File(result.RecordingFile, "audio/mpeg", result.FileName);
    }
}
