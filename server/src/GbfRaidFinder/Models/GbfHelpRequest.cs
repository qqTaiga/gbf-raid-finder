namespace GbfRaidFinder.Models;

public class GbfHelpRequest
{
    public Language Lang { get; init; }
    public string CreatedAt { get; init; }
    public string BossName { get; init; }
    public int Level { get; init; }
    public string RaidCode { get; init; }
    public string ImageUrl { get; init; }

    public GbfHelpRequest(Language lang,
        string createdAt,
        string bossName,
        int level,
        string raidCode,
        string imageUrl)
    {
        Lang = lang;
        CreatedAt = createdAt;
        BossName = bossName;
        Level = level;
        RaidCode = raidCode;
        ImageUrl = imageUrl;
    }
}
