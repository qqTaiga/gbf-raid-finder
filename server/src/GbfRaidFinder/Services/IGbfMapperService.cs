namespace GbfRaidFinder.Services;

public interface IGbfMapperService
{
    /// <summary>
    /// Some boss may have different image for different language.
    /// This dict use to map boss' eng image perceptual hash to jap image perceptual hash.
    /// Key is boss eng name, and Value is jap perceptual hash.
    /// </summary>
    Dictionary<string, string> BossEngNameJapHash { get; init; }

    /// <summary>
    /// Get boss jap image perceptual hash according to <paramref name="bossEngName"/>
    /// If <c>PerceptualHashMap</c> doesn't have the <paramref name="bossEngName"/> in key
    /// or <paramref name="lang"/> is not <c>Language. English</c>, return 0.
    /// </summary>
    /// <param name="bossEngName">Boss eng name</param>
    /// <param name="lang">Language of the tweet</param>
    /// <returns>Perceptual hash</returns>
    string TryMapToJapPerceptualHash(string bossEngName, Language lang);
}
