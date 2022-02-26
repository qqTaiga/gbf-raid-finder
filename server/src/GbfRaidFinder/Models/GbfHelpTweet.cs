using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweet
{
    public GbfHelpTweetAttachment Attachments { init; get; }
    public string Created_at { get; init; }
    public string Id { get; init; }
    public string Text { get; init; }

    [JsonConstructor]
    public GbfHelpTweet(GbfHelpTweetAttachment attachments,
        string created_at,
        string id,
        string text)
    {
        Attachments = attachments;
        Created_at = created_at;
        Id = id;
        Text = text;
    }
}
