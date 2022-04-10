using GbfRaidFinder.Models;
using GbfRaidFinder.Models.Dtos;

namespace GbfRaidFinder.Services;

public interface IInMemBossesService
{
    /// <summary>
    /// The collection of raid bosses.
    /// </summary>
    Dictionary<string, GbfRaidBoss> Bosses { get; init; }

    /// <summary>
    /// Maximum number of raid codes that can be stored for each boss.
    /// </summary>
    int MAXRAIDCODECOUNT { get; }

    /// <summary>
    /// Add <paramref name="raidBoss"/> to <c>Bosses</c> dictionary.
    /// </summary>
    /// <param name="raidBoss">New raid boss</param>
    bool AddRaidBoss(GbfRaidBoss raidBoss);

    /// <summary>
    /// Add <paramref name="raidCode"/> to boss with <paramref name="perceptualHash"/>.
    /// </summary>
    /// <param name="raidCode">Raid Code</param>
    /// <param name="perceptualHash">Perceptual hash of the boss image</param>
    void AddRaidCode(string perceptualHash, GbfRaidCode raidCode);

    /// <summary>
    /// List saved bosses.
    /// </summary>
    /// <returns>The list of raid bosses saved in Bosses</returns>
    IEnumerable<GbfRaidBossDto> ListRaidBosses();
}
