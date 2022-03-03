using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweetMedia
{
    public string Media_Keys { get; init; }
    public string Type { get; init; }
    public string Url { get; init; }


    [JsonConstructor]
    public GbfHelpTweetMedia(string media_keys, string type, string url)
    {
        Media_Keys = media_keys;
        Type = type;
        Url = url;
    }
}
