using GbfRaidFinder.Hubs;
using GbfRaidFinder.Models;
using GbfRaidFinder.Services;
using Microsoft.AspNetCore.SignalR;

namespace GbfRaidFinder.BackgroundTasks;

public class GbfConnectStreamTask : BackgroundService
{
    private readonly ILogger<GbfConnectStreamTask> _log;
    private readonly IGbfMapperService _gbfMapperService;
    private readonly IGbfRaidService _gbfRaidService;
    private readonly IHubContext<GbfRaidHub, IGbfRaidHub> _gbfRaidHub;
    private readonly IInMemBossesService _inMemService;
    private readonly ITwitterFilteredStreamService _twitterFSService;

    public GbfConnectStreamTask(ILogger<GbfConnectStreamTask> log,
        IGbfMapperService gbfMapperService,
        IGbfRaidService gbfRaidService,
        IHubContext<GbfRaidHub, IGbfRaidHub> gbfRaidHub,
        IInMemBossesService inMemService,
        ITwitterFilteredStreamService twitterFSService)
    {
        _log = log;
        _gbfMapperService = gbfMapperService;
        _gbfRaidService = gbfRaidService;
        _gbfRaidHub = gbfRaidHub;
        _inMemService = inMemService;
        _twitterFSService = twitterFSService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await foreach (var tweet in _twitterFSService.ConnectStreamAsync())
        {
            await RunMainTask(tweet);
        }
    }

    public async Task RunMainTask(GbfHelpTweet tweet)
    {
        try
        {
            var req = _gbfRaidService.ConvertGbfHelpTweetToRequest(tweet);
            var raidBosses = _inMemService.Bosses.Values;
            bool isNameExist = false;
            foreach (var boss in raidBosses)
            {
                if (req.Lang == Language.English
                    ? String.Equals(boss.EngName, req.BossName)
                    : String.Equals(boss.JapName, req.BossName))
                {
                    isNameExist = true;
                    boss.LastEncounterAt = DateTime.Now;
                    GbfRaidCode raidCode = new(req.RaidCode, req.CreatedAt);

                    _inMemService.AddRaidCode(boss.PerceptualHash, raidCode);

                    await _gbfRaidHub.Clients.Group(boss.PerceptualHash.ToString())
                        .ReceiveRaidCode(boss.PerceptualHash.ToString(), raidCode);

                    break;
                }
            }

            if (isNameExist)
                return;

            var perceptualHash = _gbfMapperService.TryMapToJapPerceptualHash(req.BossName,
                req.Lang);

            if (perceptualHash == 0)
                perceptualHash =
                    await _gbfRaidService.GetImagePerceptualHashAsync(req.ImageUrl);

            if (_inMemService.Bosses.ContainsKey(perceptualHash))
            {
                var boss = _inMemService.Bosses[perceptualHash];
                boss.LastEncounterAt = DateTime.Now;
                if (req.Lang == Language.English)
                    boss.EngName = req.BossName;
                else
                    boss.JapName = req.BossName;
                GbfRaidCode raidCode = new(req.RaidCode, req.CreatedAt);

                _inMemService.AddRaidCode(boss.PerceptualHash, raidCode);

                await _gbfRaidHub.Clients.Group(boss.PerceptualHash.ToString())
                    .ReceiveRaidCode(boss.PerceptualHash.ToString(), raidCode);
            }
            else
            {
                GbfRaidCode raidCode = new(req.RaidCode, req.CreatedAt);
                GbfRaidBoss newBoss = new(perceptualHash, raidCode);
                if (req.Lang == Language.English)
                    newBoss.EngName = req.BossName;
                else
                    newBoss.JapName = req.BossName;

                var isAdded = _inMemService.AddRaidBoss(newBoss);
                if (isAdded)
                {
                    await _gbfRaidHub.Clients.Group(perceptualHash.ToString())
                        .ReceiveRaidCode(perceptualHash.ToString(), raidCode);
                }
                else
                {
                    _log.LogWarning($"Cannot add boss- {req.BossName}, hash- {perceptualHash}");
                }
            }
        }
        catch (ArgumentException ex)
        {
            _log.LogWarning(ex, tweet.Data.Text + " =>");
        }
    }

}
