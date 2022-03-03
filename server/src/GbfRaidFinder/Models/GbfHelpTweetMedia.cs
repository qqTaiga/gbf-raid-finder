using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweetMedia
{
    public string Media_Key { get; init; }
    public string Type { get; init; }
    public string Url { get; init; }

    [JsonConstructor]
    public GbfHelpTweetMedia(string media_key, string type, string url)
    {
        Media_Key = media_key;
        Type = type;
        Url = url;
    }
}
