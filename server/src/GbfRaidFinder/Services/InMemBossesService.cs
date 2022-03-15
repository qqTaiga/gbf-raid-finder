using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

public class InMemBossesService : IInMemBossesService
{
    public Dictionary<ulong, GbfRaidBoss> Bosses { get; init; }

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

    public void AddRaidCode(ulong perceptualHash, GbfRaidCode raidCode)
    {
        var codes = Bosses[perceptualHash].RaidCodes;
        if (codes.Count >= 5)
            codes.Dequeue();
        codes.Enqueue(raidCode);
    }
}
