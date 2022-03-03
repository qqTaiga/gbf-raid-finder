using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweetExpansion
{
    public GbfHelpTweetMedia[] Media;

    [JsonConstructor]
    public GbfHelpTweetExpansion(GbfHelpTweetMedia[] media)
    {
        Media = media;
    }
}
