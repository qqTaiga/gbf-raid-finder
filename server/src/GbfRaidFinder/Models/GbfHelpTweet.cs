using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweet
{
    public GbfHelpTweetExpansion Includes { init; get; }
    public string Created_at { get; init; }
    public string Id { get; init; }
    public string Text { get; init; }

    [JsonConstructor]
    public GbfHelpTweet(GbfHelpTweetExpansion includes,
        string created_at,
        string id,
        string text)
    {
        Includes = includes;
        Created_at = created_at;
        Id = id;
        Text = text;
    }
}
