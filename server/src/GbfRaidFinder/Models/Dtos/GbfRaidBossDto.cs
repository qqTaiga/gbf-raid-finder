namespace GbfRaidFinder.Models.Dtos;

public record GbfRaidBossDto(
    string PerceptualHash,
    string? EngName,
    string? JapName,
    int Level);
