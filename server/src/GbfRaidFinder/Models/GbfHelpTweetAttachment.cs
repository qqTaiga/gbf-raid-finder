using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweetAttachment
{
    public string[] Media_keys { get; init; }

    [JsonConstructor]
    public GbfHelpTweetAttachment(string[] media_keys)
    {
        Media_keys = media_keys;
    }
}
