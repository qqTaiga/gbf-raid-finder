using GbfRaidFinder.Services;
using Microsoft.AspNetCore.Mvc;

namespace GbfRaidFinder.Controllers;

[Route("gbf")]
[ApiController]
public class GbfRFController : ControllerBase
{
    private readonly ILogger<GbfRFController> _log;
    private readonly IInMemBossesService _inMemRaidBossesService;
    private readonly IGbfRaidService _gbfRaidService;
    private readonly ITwitterFilteredStreamService _twitterFSService;

    public GbfRFController(ILogger<GbfRFController> log,
        IInMemBossesService inMemRaidBossesService,
        IGbfRaidService gbfRaidService,
        ITwitterFilteredStreamService twitterFSService)
    {
        _log = log;
        _inMemRaidBossesService = inMemRaidBossesService;
        _gbfRaidService = gbfRaidService;
        _twitterFSService = twitterFSService;
    }

    /// <summary>
    /// Fetch image from the <paramref name="url"/> and calculate the perceptual hash
    /// </summary>
    /// <param name="url">The twitter url of the image</param>
    /// <response code="200">Return perceptual hash</response>
    /// <response code="400">Error message</response>
    [HttpGet("perceptual-hash")]
    public async Task<IActionResult> CalculateImagePerceptualHash(string url)
    {
        var hash = await _gbfRaidService.GetImagePerceptualHashAsync(url);

        if (string.IsNullOrWhiteSpace(hash))
            return BadRequest(new { error = "Bad or invalid twitter image url" });

        return Ok(hash);
    }

    /// <summary>
    /// Get raid bosses list
    /// </summary>
    /// <response code="200">Return bosses list</response>
    [HttpGet("list")]
    public IActionResult ListRaidBosses()
    {
        var bossList = _inMemRaidBossesService.ListRaidBosses();

        return Ok(bossList);
    }
}
