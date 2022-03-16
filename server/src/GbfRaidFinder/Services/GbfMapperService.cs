namespace GbfRaidFinder.Services;

public class GbfMapperService : IGbfMapperService
{
    public Dictionary<string, ulong> BossEngNameJapHash { get; init; }

    public GbfMapperService()
    {
        BossEngNameJapHash = new();
        BossEngNameJapHash.Add("Lvl 120 Medusa", 17682549972253862000);
    }

    public ulong TryMapToJapPerceptualHash(string bossEngName, Language lang)
    {
        if (lang != Language.English || !BossEngNameJapHash.ContainsKey(bossEngName))
            return 0;

        return BossEngNameJapHash[bossEngName];
    }
}
