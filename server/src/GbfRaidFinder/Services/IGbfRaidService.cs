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
}
