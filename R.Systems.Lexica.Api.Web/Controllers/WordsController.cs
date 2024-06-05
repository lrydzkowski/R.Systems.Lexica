using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Queries.GetDefinitions;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[Route("words")]
public class WordsController : ApiControllerWithAuthBase
{
    public WordsController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get word definitions")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        description: "Correct response",
        type: typeof(List<Definition>),
        contentTypes: [MediaTypeNames.Application.Json]
    )]
    [HttpGet("{word}/definitions")]
    public async Task<IActionResult> GetDefinitions(
        string word,
        CancellationToken cancellationToken
    )
    {
        GetDefinitionsResult result = await Mediator.Send(
            new GetDefinitionsQuery { Word = word },
            cancellationToken
        );

        return Ok(result.Definitions);
    }
}
