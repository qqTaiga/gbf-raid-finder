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

    [HttpPost("filtered-stream/modify")]
    public async Task<IActionResult> ModifyRule(TwitterFilteredStreamRuleDto input)
    {
        HttpResult result = await _twitterFSService.ModifyRule(
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
}
