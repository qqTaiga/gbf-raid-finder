using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

interface IGbfRaidService
{
    /// <summary>
    /// Convert GbfHelpTweet to GbfHelpRequest
    /// </summary>
    /// <param name="tweet">GbfHelpTweet get from stream</param>
    /// <returns>GbfHelpRequest</returns>
    /// <exception cref="ArgumentException">Missing or invalid value</exception>
    GbfHelpRequest ConvertGbfHelpTweetToRequest(GbfHelpTweet tweet);

    /// <summary>
    /// Get image from <paramref name="url"/>'s Twitter media link, use top 75% of image to 
    /// calculate perceptual hash. Return 0 if the <paramref name="url"/> is blank, is invalid
    /// Twitter media link or failed to get the image.
    /// </summary>
    /// <param name="url">Twitter media link</param>
    /// <returns>Perceptual hash of top 75% of image</returns>
    Task<ulong> GetImagePerceptualHashAsync(string url);
}
