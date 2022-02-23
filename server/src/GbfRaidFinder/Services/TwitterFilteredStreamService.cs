using System.Text.Json;
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

    public async Task<HttpResult> ModifyRule(TwitterFilteredStreamRuleActions action,
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
                delete = new { delete = rules }
            });
        }
        if (content != null)
        {
            content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        }

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            _urls.TwitterFilteredStreamRules)
        {
            Headers =
            {
                { HeaderNames.Authorization, "Bearer" + _keys.TwitterJwtToken }
            },
            Content = content
        };

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return new HttpResult(true);
        }
        else
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            HttpResult result = new(false)
            {
                ErrorDesc = await JsonSerializer.Deserialize<dynamic>(contentStream)
            };
            return result;
        }
    }
}

