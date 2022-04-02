namespace GbfRaidFinder.Services;

public class GbfMapperService : IGbfMapperService
{
    public Dictionary<string, string> BossEngNameJapHash { get; init; }

    public GbfMapperService()
    {
        BossEngNameJapHash = new();
        BossEngNameJapHash.Add("Lvl 120 Medusa", "17682549972253862360");
    }

    public string TryMapToJapPerceptualHash(string bossEngName, Language lang)
    {
        if (lang != Language.English || !BossEngNameJapHash.ContainsKey(bossEngName))
            return "";

        return BossEngNameJapHash[bossEngName];
    }
}
