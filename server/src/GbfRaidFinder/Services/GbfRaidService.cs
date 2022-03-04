using GbfRaidFinder.Models;

namespace GbfRaidFinder.Services;

public class GbfRaidService : IGbfRaidService
{
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

}
