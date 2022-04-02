using CoenM.ImageHash.HashAlgorithms;
using GbfRaidFinder.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GbfRaidFinder.Services;

public class GbfRaidService : IGbfRaidService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GbfRaidService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public GbfHelpRequest ConvertGbfHelpTweetToRequest(GbfHelpTweet tweet)
    {
        if (tweet.Includes.Media == null ||
            tweet.Includes.Media.Count() != 1 ||
            tweet.Includes.Media[0].Type != "photo")
            throw new ArgumentException("Missing or invalid media");

        if (tweet.Data.Created_At == null)
            throw new ArgumentException(nameof(GbfHelpTweetData.Created_At) + " is null");
        if (tweet.Data.Text == null)
            throw new ArgumentException(nameof(GbfHelpTweetData.Text) + " is null");
        if (tweet.Includes.Media[0].Url == null)
            throw new ArgumentException(nameof(GbfHelpTweetMedia.Url) + " is null");

        var texts = tweet.Data.Text.Split("\n");
        if (texts.Length < 4 ||
            (!(texts[^4].Contains(":参戦ID") && texts[^3].Contains("参加者募集！")) &&
            (!texts[^4].Contains(":Battle ID") && !texts[^3].Contains("I need backup!"))))
            throw new ArgumentException("Invalid text content");

        var lang = texts[^4].Contains(":参戦ID") ? Language.Japanese : Language.English;
        var raidCodeLine = texts[^4].Split(" ");
        if ((lang == Language.Japanese && raidCodeLine.Length < 2) &&
            (lang == Language.English && raidCodeLine.Length < 3))
            throw new ArgumentException("Invalid text content");

        var createdAt = tweet.Data.Created_At;
        var bossName = texts[^2];
        var raidCode = lang == Language.Japanese ? raidCodeLine[^2] : raidCodeLine[^3];
        var imageUrl = tweet.Includes.Media[0].Url;

        return new GbfHelpRequest(lang, createdAt, bossName, raidCode, imageUrl);
    }

    public async Task<string> GetImagePerceptualHashAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "";

        if (!url.StartsWith("https://pbs.twimg.com/media/"))
            return "";

        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return "";

        var imageBytes = await response.Content.ReadAsByteArrayAsync();
        using var pic = Image.Load<Rgba32>(imageBytes);
        var width = pic.Width;
        var croppedHeight = (int)(pic.Height * 0.75);
        pic.Mutate(_ => _.Crop(width, croppedHeight));

        return new PerceptualHash().Hash(pic).ToString();
    }
}
