using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

public class InMemBossesService : IInMemBossesService
{
    public Dictionary<string, GbfRaidBoss> Bosses { get; init; }
    public int MAXRAIDCODECOUNT => 7;

    public InMemBossesService()
    {
        Bosses = new();
    }

    public bool AddRaidBoss(GbfRaidBoss raidBoss)
    {
        if (Bosses.ContainsKey(raidBoss.PerceptualHash))
            return false;

        Bosses.Add(raidBoss.PerceptualHash, raidBoss);
        return true;
    }

    public void AddRaidCode(string perceptualHash, GbfRaidCode raidCode)
    {
        var codes = Bosses[perceptualHash].RaidCodes;
        if (codes.Count >= MAXRAIDCODECOUNT)
            codes.Dequeue();
        codes.Enqueue(raidCode);
    }
}
