using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R.Systems.Lexica.Core.Models;
using R.Systems.Lexica.Core.Services;
using R.Systems.Shared.Core.Validation;

namespace R.Systems.Lexica.WebApi.Controllers;

[ApiController, Route("sets")]
public class SetController : ControllerBase
{
    public SetController(
        SetReadService setReadService,
        ValidationResult validationResult)
    {
        SetReadService = setReadService;
        ValidationResult = validationResult;
    }

    public SetReadService SetReadService { get; }
    public ValidationResult ValidationResult { get; }

    [HttpGet, Route("{setName}"), Authorize(Roles = "lexica")]
    public async Task<IActionResult> Get(string setName)
    {
        OperationResult<Set?> operationResult = await SetReadService.GetAsync(setName);
        if (!operationResult.Result)
        {
            return BadRequest(ValidationResult.Errors);
        }
        return Ok(operationResult.Data);
    }

    [HttpGet, Authorize(Roles = "lexica")]
    public async Task<IActionResult> Get()
    {
        OperationResult<List<Set>> operationResult = await SetReadService.GetAsync();
        if (!operationResult.Result)
        {
            return BadRequest(ValidationResult.Errors);
        }
        return Ok(operationResult.Data);
    }
}
