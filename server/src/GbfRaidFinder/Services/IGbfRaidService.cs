using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

public interface IGbfRaidService
{
    /// <summary>
    /// Convert GbfHelpTweet to GbfHelpRequest
    /// </summary>
    /// <param name="tweet">GbfHelpTweet get from stream</param>
    /// <returns>GbfHelpRequest</returns>
    /// <exception cref="ArgumentException">Missing or invalid value</exception>
    public GbfHelpRequest ConvertGbfHelpTweetToRequest(GbfHelpTweet tweet);

    /// <summary>
    /// Get image from <paramref name="req"/>'s ImageUrl, use top 75% of image to calculate
    /// perceptual hash. Return 0 if the ImageUrl is null or cannot get the image.
    /// </summary>
    /// <param name="req">GbfHelpRequest</param>
    /// <returns>Perceptual hash of top 75% of image</returns>
    public Task<ulong> GetImagePerceptualHashAsync(string url);
}
