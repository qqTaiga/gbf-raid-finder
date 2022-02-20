using System.Collections.ObjectModel;

namespace GbfRaidFinder.Models;

public class HttpResult
{
    public bool IsSuccess { get; set; }
    public Collection<string>? ErrorDesc { get; set; }
}
