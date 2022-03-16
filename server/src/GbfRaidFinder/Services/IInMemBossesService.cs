using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

interface IInMemBossesService
{
    /// <summary>
    /// The collection of raid bosses.
    /// </summary>
    Dictionary<ulong, GbfRaidBoss> Bosses { get; init; }

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
    void AddRaidCode(ulong perceptualHash, GbfRaidCode raidCode);
}
