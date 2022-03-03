using System.ComponentModel.DataAnnotations;
using GbfRaidFinder.Models.Enums;

namespace GbfRaidFinder.Models.Dtos;

public record TwitterFilteredStreamRuleDto(
    [Required] TwitterFilteredStreamRuleActions Action,
    [Required] bool DryRun,
    TwitterFilteredStreamRule[]? Rules,
    string[]? Ids);
