using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.Core.Sets.Commands.CreateSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;

namespace R.Systems.Lexica.WebApi.Controllers;

[ApiController, Route("sets")]
public class SetController : ControllerBase
{
    public SetController(ISender mediator)
    {
        Mediator = mediator;
    }

    public ISender Mediator { get; }

    [HttpGet, Authorize(Roles = "lexica")]
    public async Task<IActionResult> Get()
    {
        List<Set> sets = await Mediator.Send(new GetSetsQuery());
        return Ok(sets);
    }

    [HttpGet, Route("{setName}"), Authorize(Roles = "lexica")]
    public async Task<IActionResult> Get(string setName)
    {
        Set set = await Mediator.Send(new GetSetQuery { SetName = setName });
        return Ok(set);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSetCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
