using GbfRaidFinder.Models;
using GbfRaidFinder.Services;

namespace GbfRaidFinder.BackgroundTasks;

public class GbfConnectStreamTask : BackgroundService
{
    private readonly ILogger<GbfConnectStreamTask> _log;
    private readonly IGbfRaidService _gbfRaidService;
    private readonly IInMemBossesService _inMemService;
    private readonly ITwitterFilteredStreamService _twitterFSService;

    public GbfConnectStreamTask(ILogger<GbfConnectStreamTask> log,
        IGbfRaidService gbfRaidService,
        IInMemBossesService inMemService,
        ITwitterFilteredStreamService twitterFSService)
    {
        _log = log;
        _gbfRaidService = gbfRaidService;
        _inMemService = inMemService;
        _twitterFSService = twitterFSService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        await foreach (var tweet in _twitterFSService.ConnectStreamAsync())
        {
            try
            {
                var req = _gbfRaidService.ConvertGbfHelpTweetToRequest(tweet);
                var raidBosses = _inMemService.Bosses.Values;
                bool isNameExist = false;
                for (int i = 0; i < raidBosses.Count(); i++)
                {
                    var boss = raidBosses.ElementAt(i);
                    if (req.Lang == Language.English
                        ? boss.EngName == req.BossName
                        : boss.JapName == req.BossName)
                    {
                        isNameExist = true;
                        boss.LastEncounterAt = DateTime.Now;
                        _inMemService.AddRaidCode(boss.PerceptualHash,
                            new(req.RaidCode, req.CreatedAt));
                        // TODO: add send latest code to client

                        break;
                    }
                }

                if (isNameExist)
                    continue;

                var perceptualHash = await
                    _gbfRaidService.GetImagePerceptualHashAsync(req.ImageUrl);
                // TODO: add mapper to map perceptual hash for some boss with different image in different language

                if (_inMemService.Bosses.ContainsKey(perceptualHash))
                {
                    var boss = _inMemService.Bosses[perceptualHash];
                    boss.LastEncounterAt = DateTime.Now;
                    if (req.Lang == Language.English)
                        boss.EngName = req.BossName;
                    else
                        boss.JapName = req.BossName;

                    _inMemService.AddRaidCode(boss.PerceptualHash,
                        new(req.RaidCode, req.CreatedAt));
                    // TODO: add send latest code to client
                }
                else
                {
                    GbfRaidBoss newBoss = new(perceptualHash, new(req.RaidCode, req.CreatedAt));
                    if (req.Lang == Language.English)
                        newBoss.EngName = req.BossName;
                    else
                        newBoss.JapName = req.BossName;

                    var isAdded = _inMemService.AddRaidBoss(newBoss);
                    if (!isAdded)
                        _log.LogWarning($"Cannot add boss- {req.BossName}, hash- {perceptualHash}");
                }
            }
            catch (ArgumentException ex)
            {
                _log.LogWarning(ex, tweet.Data.Text + " =>");
            }
        }
    }
}
