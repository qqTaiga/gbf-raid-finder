using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class GbfHelpTweetData
{
    public string Created_At { get; init; }
    public string Id { get; init; }
    public string Text { get; init; }

    [JsonConstructor]
    public GbfHelpTweetData(string created_at, string id, string text)
    {
        Created_At = created_at;
        Id = id;
        Text = text;
    }
}
