using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Version.Queries.GetVersion;

namespace R.Systems.Lexica.WebApi.Controllers;

[ApiController]
[Route("version")]
public class VersionController : ControllerBase
{
    public VersionController(ISender mediator)
    {
        Mediator = mediator;
    }

    public ISender Mediator { get; }

    [HttpGet]
    public async Task<IActionResult> GetVersion()
    {
        string? version = await Mediator.Send(new GetVersionQuery());
        return Ok(version);
    }
}
