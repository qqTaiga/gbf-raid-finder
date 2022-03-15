using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

public interface IInMemBossesService
{
    /// <summary>
    /// The collection of raid bosses.
    /// </summary>
    Dictionary<ulong, GbfRaidBoss> Bosses { get; init; }

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
    void AddRaidCode(ulong perceptualHash, GbfRaidCode raidCode);
}
