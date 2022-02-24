using System.Text.Json.Serialization;

namespace GbfRaidFinder.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TwitterFilteredStreamRuleActions
{
    Add,
    Delete
}
