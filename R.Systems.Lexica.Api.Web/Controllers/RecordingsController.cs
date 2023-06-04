using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Api.Web.Mappers;
using R.Systems.Lexica.Api.Web.Models;
using R.Systems.Lexica.Core.Queries.GetRecording;
using R.Systems.Lexica.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[Route("recordings")]
public class RecordingsController : ControllerBase
{
    private readonly ISender _mediator;

    public RecordingsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [SwaggerOperation(Summary = "Get recording")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(byte[]),
        contentTypes: new[] { "audio/mpeg" }
    )]
    [SwaggerResponse(statusCode: 500)]
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
