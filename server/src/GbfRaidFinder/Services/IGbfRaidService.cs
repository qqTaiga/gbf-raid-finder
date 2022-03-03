using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

public interface IGbfRaidService
{
    /// <summary>
    /// Convert GbfHelpTweet to GbfHelpRequest
    /// </summary>
    /// <param name="tweet">GbfHelpTweet get from stream</param>
    /// <returns>GbfHelpRequest</returns>
    public GbfHelpRequest ConvertGbfHelpTweetToRequest(GbfHelpTweet tweet);
}
