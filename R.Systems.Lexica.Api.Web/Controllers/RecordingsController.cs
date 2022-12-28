using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Lexica.Core.Recordings.Queries.GetRecording;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
[Route("recordings")]
public class RecordingsController : ControllerBase
{
    public RecordingsController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get recording")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(byte[]),
        contentTypes: new[] { "audio/mpeg" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [Authorize, RequiredScope("Access")]
    [HttpGet, Route("{word}")]
    public async Task<IActionResult> GetRecording([FromRoute] string word)
    {
        GetRecordingResult result = await Mediator.Send(new GetRecordingQuery { Word = word });
        if (result.RecordingFile == null)
        {
            return NotFound(null);
        }

        return File(result.RecordingFile, "audio/mpeg", result.FileName);
    }
}
