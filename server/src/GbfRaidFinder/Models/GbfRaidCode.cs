namespace GbfRaidFinder.Models;

public class GbfRaidCode
{
    public string Code { get; init; }
    public string CreatedAt { get; init; }

    public GbfRaidCode(string code, string createdAt)
    {
        Code = code;
        CreatedAt = createdAt;
    }
}
