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
    private readonly ILogger<TwitterFilteredStreamService> _log;
    private readonly Keys _keys;
    private readonly Urls _urls;

    public TwitterFilteredStreamService(IHttpClientFactory httpClientFactory,
        ILogger<TwitterFilteredStreamService> log,
        IOptions<Keys> keys,
        IOptions<Urls> urls)
    {
        _httpClientFactory = httpClientFactory;
        _log = log;
        _keys = keys.Value;
        _urls = urls.Value;
    }

    public async Task<HttpResult> ModifyRulesAsync(TwitterFilteredStreamRuleActions action,
            bool dryRun,
            TwitterFilteredStreamRule[]? rules,
            string[]? ids)
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
            var idsParam = new { ids = ids };
            content = JsonContent.Create(new
            {
                delete = idsParam
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
        var contentStream = await response.Content.ReadAsStreamAsync();

        if (response.IsSuccessStatusCode)
        {
            HttpResult result = new(true)
            {
                Content = await JsonSerializer.DeserializeAsync<dynamic>(contentStream)
            };
            return result;
        }
        else
        {
            HttpResult result = new(false)
            {
                ErrorDesc = await JsonSerializer.DeserializeAsync<dynamic>(contentStream)
            };
            return result;
        }
    }

    public async Task<HttpResult> RetrieveRulesAsync()
    {
        var url = _urls.TwitterFilteredStreamRule;
        var request = new HttpRequestMessage(HttpMethod.Get, url)
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

    public async IAsyncEnumerable<GbfHelpTweet> ConnectStreamAsync()
    {
        var url = _urls.TwitterFilteredStream;
        // Retrieve created at and media key fields
        url += "?tweet.fields=created_at&media.fields=url&expansions=attachments.media_keys";
        var request = new HttpRequestMessage(HttpMethod.Get, url)
        {
            Headers =
            {
                { HeaderNames.Authorization, "Bearer " + _keys.TwitterJwtToken }
            },
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
        var response = await httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.StatusCode.ToString());

        var contentStream = await response.Content.ReadAsStreamAsync();
        using (var reader = new StreamReader(contentStream))
        {
            string? line = null;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line == null)
                    continue;

                var tweet = JsonSerializer.Deserialize<GbfHelpTweet>(line);

                if (tweet?.Text != null)
                    yield return tweet;
            }
            _log.LogWarning("connection closed => " + line);
        }

    }
}

