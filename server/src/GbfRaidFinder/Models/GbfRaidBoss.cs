namespace GbfRaidFinder.Models;

public class GbfRaidBoss
{
    public ulong PerceptualHash { get; init; }
    public string? EngName { get; set; }
    public string? JapName { get; set; }
    public DateOnly LastEncounterAt { get; set; }
    public Queue<GbfRaidCode> RaidCodes { get; init; }

    public GbfRaidBoss(ulong perceptualHash, GbfRaidCode raidCode)
    {
        PerceptualHash = perceptualHash;
        LastEncounterAt = DateOnly.FromDateTime(DateTime.Now);
        RaidCodes = new Queue<GbfRaidCode>();
        RaidCodes.Enqueue(raidCode);
    }
}
