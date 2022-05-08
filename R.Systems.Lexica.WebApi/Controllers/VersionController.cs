using MediatR;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Common.Models;
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
        App app = await Mediator.Send(new GetVersionQuery());
        return Ok(app);
    }
}
