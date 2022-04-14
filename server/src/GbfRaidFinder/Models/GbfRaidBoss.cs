namespace GbfRaidFinder.Models;

public class GbfRaidBoss
{
    public string PerceptualHash { get; init; }
    public string? EngName { get; set; }
    public string? JapName { get; set; }
    public int Level { get; init; }
    public DateTime LastEncounterAt { get; set; }
    public Queue<GbfRaidCode> RaidCodes { get; init; }

    public GbfRaidBoss(string perceptualHash, int level, GbfRaidCode raidCode)
    {
        PerceptualHash = perceptualHash;
        Level = level;
        LastEncounterAt = DateTime.Now;
        RaidCodes = new Queue<GbfRaidCode>();
        RaidCodes.Enqueue(raidCode);
    }
}
