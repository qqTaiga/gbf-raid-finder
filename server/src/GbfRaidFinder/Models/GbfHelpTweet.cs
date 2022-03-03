using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweet
{
    public GbfHelpTweetData Data { get; init; }
    public GbfHelpTweetExpansion Includes { get; init; }

    [JsonConstructor]
    public GbfHelpTweet(GbfHelpTweetData data, GbfHelpTweetExpansion includes)
    {
        Data = data;
        Includes = includes;
    }
}
