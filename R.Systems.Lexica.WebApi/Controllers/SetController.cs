using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Common.Models;
using R.Systems.Lexica.Core.Sets.Queries.GetSet;
using R.Systems.Lexica.Core.Sets.Queries.GetSets;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.WebApi.Controllers;

[ApiController, Route("sets")]
public class SetController : ControllerBase
{
    public SetController(
        ValidationResult validationResult,
        GetSetsQuery getSetsQuery,
        GetSetQuery getSetQuery)
    {
        ValidationResult = validationResult;
        GetSetsQuery = getSetsQuery;
        GetSetQuery = getSetQuery;
    }
    public ValidationResult ValidationResult { get; }
    public GetSetsQuery GetSetsQuery { get; }
    public GetSetQuery GetSetQuery { get; }

    [HttpGet, Authorize(Roles = "lexica")]
    public async Task<IActionResult> Get()
    {
        List<Set> sets = await GetSetsQuery.GetAsync();
        return Ok(sets);
    }

    [HttpGet, Route("{setName}"), Authorize(Roles = "lexica")]
    public async Task<IActionResult> Get(string setName)
    {
        Set set = await GetSetQuery.GetAsync(setName);
        return Ok(set);
    }
}
