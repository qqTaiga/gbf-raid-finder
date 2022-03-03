namespace GbfRaidFinder.Models;

public class HttpResult
{
    public bool IsSuccess { get; init; }
    public dynamic? Content { get; init; }
    public dynamic? ErrorDesc { get; init; }

    public HttpResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}
