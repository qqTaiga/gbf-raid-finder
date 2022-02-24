using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models;

public class TwitterFilteredStreamRule
{
    public string Value { get; set; }
    public string? Tag { get; set; }


    public TwitterFilteredStreamRule(string value)
    {
        Value = value;
    }

    [JsonConstructor]
    public TwitterFilteredStreamRule(string value, string tag)
    {
        Value = value;
        Tag = tag;
    }
}
