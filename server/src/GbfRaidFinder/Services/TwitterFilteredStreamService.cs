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

    public async Task<HttpResult> ModifyRulesAsync(TwitterFilteredStreamRuleActions action,
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
        url += "?tweet.fields=created_at&expansions=attachments.media_keys";
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
        if (response.IsSuccessStatusCode)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(contentStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        continue;

                    var tweet = JsonSerializer.Deserialize<JsonElement>(line);
                    GbfHelpTweet? gbfTweet = null;
                    try
                    {
                        gbfTweet = tweet.GetProperty("data").Deserialize<GbfHelpTweet>(
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch (KeyNotFoundException ex)
                    {
                        // TODO: change to serilog
                        Console.WriteLine(ex.Message);
                        continue;
                    }

                    if (gbfTweet?.Text != null)
                        yield return gbfTweet;
                }
                // TODO: log connection end and last line
            }
        }
        else
        {
            throw new HttpRequestException(response.StatusCode.ToString());
        }

    }
}

