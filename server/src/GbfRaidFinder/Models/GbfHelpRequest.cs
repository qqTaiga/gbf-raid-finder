namespace GbfRaidFinder.Models;

public class GbfHelpRequest
{
    public DateTime CreatedAt;
    public string BossName;
    public string RaidCode;
    public string ImageUrl;


    public GbfHelpRequest(DateTime createdAt,
        string bossName,
        string raidCode,
        string imageUrl)
    {
        CreatedAt = createdAt;
        BossName = bossName;
        RaidCode = raidCode;
        ImageUrl = imageUrl;
    }
}
