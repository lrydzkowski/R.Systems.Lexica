using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using R.Systems.Lexica.Core.Queries.GetDefinitions;
using R.Systems.Lexica.Infrastructure.Azure;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Lexica.Api.Web.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = AuthenticationSchemes.AzureAd)]
[Route("words")]
public class WordsController : ControllerBase
{
    public WordsController(ISender mediator)
    {
        Mediator = mediator;
    }

    private ISender Mediator { get; }

    [SwaggerOperation(Summary = "Get word definitions")]
    [SwaggerResponse(
        statusCode: 200,
        description: "Correct response",
        type: typeof(List<Definition>),
        contentTypes: new[] { "application/json" }
    )]
    [SwaggerResponse(statusCode: 500)]
    [Route("{word}/definitions")]
    [HttpGet]
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
