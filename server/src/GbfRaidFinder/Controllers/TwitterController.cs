using GbfRaidFinder.Models;
using GbfRaidFinder.Models.Dtos;
using GbfRaidFinder.Services;
using Microsoft.AspNetCore.Mvc;

namespace GbfRaidFinder.Controllers;

[Route("twitter")]
[ApiController]
public class TwitterController : ControllerBase
{
    private readonly ITwitterFilteredStreamService _twitterFSService;

    public TwitterController(ITwitterFilteredStreamService twitterFSService)
    {
        _twitterFSService = twitterFSService;
    }

    /// <summary>
    /// Modify Twitter Filtered Stream Rule (add/delete).
    /// If dryRun set to true to test syntax wihout modify current rules.
    /// </summary>
    /// <param name="input">Action, DryRun, and Rules</param>
    /// <response code="200">Success modified the rules</response>
    /// <response code="400">Missing required input</response>
    /// <response code="429">Invalid rules or syntax</response>
    [HttpPost("filtered-stream/rules/modify")]
    public async Task<IActionResult> ModifyRules(TwitterFilteredStreamRuleDto input)
    {
        HttpResult result = await _twitterFSService.ModifyRules(
            input.Action,
            input.DryRun,
            input.Rules);

        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return UnprocessableEntity(result.ErrorDesc);
        }
    }

    /// <summary>
    /// Retrieve Twitter Filtered Stream Rules.
    /// </summary>
    /// <response code="200">Return rules</response>
    /// <response code="400">Return error description</response>
    [HttpGet("filtered-stream/rules")]
    public async Task<IActionResult> RetrieveRules()
    {
        HttpResult result = await _twitterFSService.RetrieveRules();
        if (result.IsSuccess)
        {
            return Ok(result.Content);
        }
        else
        {
            return BadRequest(result.ErrorDesc);
        }
    }
}
