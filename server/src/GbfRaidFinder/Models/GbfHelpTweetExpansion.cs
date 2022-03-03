using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweetExpansion
{
    public GbfHelpTweetMedia[] Media { get; init; }

    [JsonConstructor]
    public GbfHelpTweetExpansion(GbfHelpTweetMedia[] media)
    {
        Media = media;
    }
}
