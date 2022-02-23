using GbfRaidFinder.Models;
using GbfRaidFinder.Models.Enums;

namespace GbfRaidFinder.Services;

public interface ITwitterFilteredStreamService
{
    /// <summary>
    /// Add rules to twitter filtered stream.
    /// set <paramref name="dryRun"/> to true to test a the syntax of rules without submitting it.
    /// </summary>
    /// <param name="dryRun">
    /// Set to true to test a the syntax of rule without submitting it.
    /// </param>
    /// <param name="rules">Rules</param>
    /// <returns>
    /// Return <c>HttpResult</c>
    /// </returns>
    Task<HttpResult> ModifyRule(TwitterFilteredStreamRuleActions action,
        bool dryRun,
        TwitterFilteredStreamRule[] rules);
}
