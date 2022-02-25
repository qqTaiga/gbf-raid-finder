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

    public async Task<HttpResult> ModifyRules(TwitterFilteredStreamRuleActions action,
            bool dryRun,
            TwitterFilteredStreamRule[] rules)
    {
        JsonContent? content = null;
        if (action == TwitterFilteredStreamRuleActions.Add)
        {
            content = JsonContent.Create(new
            {
                add = rules
            });
        }
        else if (action == TwitterFilteredStreamRuleActions.Delete)
        {
            content = JsonContent.Create(new
            {
                delete = rules
            });
        }
        if (content != null)
        {
            content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        }

        var url = _urls.TwitterFilteredStreamRule;
        if (dryRun)
        {
            url += "?dry_run=true";
        }
        var request = new HttpRequestMessage(
            HttpMethod.Post,
            url)
        {
            Headers =
            {
                { HeaderNames.Authorization, "Bearer " + _keys.TwitterJwtToken }
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
                ErrorDesc = await JsonSerializer.DeserializeAsync<dynamic>(contentStream)
            };
            return result;
        }
    }

    public async Task<HttpResult> RetrieveRules()
    {
        var url = _urls.TwitterFilteredStreamRule;
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            url)
        {
            Headers =
            {
                { HeaderNames.Authorization, "Bearer " + _keys.TwitterJwtToken }
            },
        };
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            HttpResult result = new(true)
            {
                Content = await JsonSerializer.DeserializeAsync<dynamic>(contentStream)
            };
            return result;
        }
        else
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            HttpResult result = new(false)
            {
                ErrorDesc = await JsonSerializer.DeserializeAsync<dynamic>(contentStream)
            };
            return result;
        }
    }
}

