using GbfRaidFinder.Models;
using GbfRaidFinder.Models.Enums;
using GbfRaidFinder.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace GbfRaidFinder.Services;

public class TwitterFilteredStreamService : ITwitterFilteredStreamService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Keys _keys;
    private readonly Urls _urls;

    public TwitterFilteredStreamService(IHttpClientFactory httpClientFactory,
        IOptions<Keys> keys,
        IOptions<Urls> urls)
    {
        _httpClientFactory = httpClientFactory;
        _keys = keys.Value;
        _urls = urls.Value;
    }

    public async Task<HttpResult> AddRule(TwitterFilteredStreamRuleActions action,
            bool dryRun,
            TwitterFilteredStreamRule[] rules)
    {
        JsonContent? content = null;
        if (action == TwitterFilteredStreamRuleActions.Add)
        {
            content = JsonContent.Create(new
            {
                add = new { add = rules }
            });
        }
        else if (action == TwitterFilteredStreamRuleActions.Delete)
        {
            content = JsonContent.Create(new
            {
                delete = new { add = rules }
            });
        }
        var httpRequestMessage = new HttpRequestMessage(
            action == TwitterFilteredStreamRuleActions.Retrieve ? HttpMethod.Get : HttpMethod.Post,
            _urls.TwitterFilteredStreamRules)
        {
            Headers =
            {
                { HeaderNames.ContentType, "application/json" },
                { HeaderNames.Authorization, "Bearer" + _keys.TwitterJwtToken }
            },
            Content = content
        };

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        // TODO
        return null;
    }
}

