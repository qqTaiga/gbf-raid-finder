namespace GbfRaidFinder.Models;

public class HttpResult
{
    public bool IsSuccess { get; set; }
    public dynamic? Content { get; set; }
    public dynamic? ErrorDesc { get; set; }

    public HttpResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}
