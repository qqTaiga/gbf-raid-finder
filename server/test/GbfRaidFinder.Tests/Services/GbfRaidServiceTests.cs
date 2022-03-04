using System;
using FluentAssertions;
using GbfRaidFinder.Models;
using GbfRaidFinder.Services;
using Xunit;

namespace GbfRaidFinder.Tests.Services;

public class GbfRaidServiceTests
{
    [Theory]
    [InlineData("A6806FCC :参戦ID\n参加者募集！\nLv100 ウリエル\nhttps://t.co/GGyX19yYAG")]
    [InlineData("abc A6806FCC :参戦ID\n参加者募集！\nLv100 ウリエル\nhttps://t.co/GGyX19yYAG")]
    [InlineData("abc \nA6806FCC :参戦ID\n参加者募集！\nLv100 ウリエル\nhttps://t.co/GGyX19yYAG")]
    public void ConvertGbfHelpTweetToRequest_JapaneseText_GetGbfHelpRequest(string text)
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T16:08:11.000Z",
            "1499416354302021635",
            text);
        GbfHelpTweetMedia media = new("3_841815632207212544",
            "photo",
            "https://pbs.twimg.com/media/C66623wU8AACyL2.jpg");
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        var raidRequest = service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        raidRequest.Should().NotBeNull();
        raidRequest.Lang.Should().Be(Language.Japanese);
        raidRequest.CreatedAt.Should().Be("2022-03-03T16:08:11.000Z");
        raidRequest.BossName.Should().Be("Lv100 ウリエル");
        raidRequest.RaidCode.Should().Be("A6806FCC");
        raidRequest.ImageUrl.Should().Be("https://pbs.twimg.com/media/C66623wU8AACyL2.jpg");
    }

    [Theory]
    [InlineData("B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b")]
    [InlineData("Abc fsdf B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b")]
    [InlineData("Test \nB1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b")]
    public void ConvertGbfHelpTweetToRequest_EnglishText_GetGbfHelpRequest(string text)
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            text);
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        var raidRequest = service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        raidRequest.Should().NotBeNull();
        raidRequest.Lang.Should().Be(Language.English);
        raidRequest.CreatedAt.Should().Be("2022-03-03T15:32:41.000Z");
        raidRequest.BossName.Should().Be("Lvl 200 Lindwurm");
        raidRequest.RaidCode.Should().Be("B1154E88");
        raidRequest.ImageUrl.Should().Be("https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
    }

    [Theory]
    [InlineData(null, "value", "value", nameof(GbfHelpTweetData.Created_At))]
    [InlineData("value", null, "value", nameof(GbfHelpTweetData.Text))]
    [InlineData("value", "value", null, nameof(GbfHelpTweetMedia.Url))]
    public void ConvertGbfHelpTweetToRequest_MissingRequiredValue_ThrowArgumentException(string createdAt,
        string text,
        string mediaUrl,
        string missingValueFieldName)
    {
        // Arrange
        GbfHelpTweetData data = new(createdAt,
            "1499407423215333378",
            text);
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            mediaUrl);
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        Action act = () => service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(missingValueFieldName + " is null");
    }

    [Fact]
    public void ConvertGbfHelpTweetToRequest_MediaCountNotEqualOne_ThrowArgumentException()
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            "B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b");
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
        GbfHelpTweetExpansion expansion = new(new[] { media, media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        Action act = () => service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Missing or invalid media");
    }

    [Fact]
    public void ConvertGbfHelpTweetToRequest_MediaTypeNotEqualToPhoto_ThrowArgumentException()
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            "B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b");
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "video",
            "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        Action act = () => service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Missing or invalid media");
    }

    [Theory]
    [InlineData("B1154E88 :Battle ID\nI need backup!Lvl 200 Lindwurm\nhttps://t.co/VylsLCm88b")]
    [InlineData("A6806FCC :参戦ID\n参加者募集！\nLv100 ウリエル\nabc\nhttps://t.co/GGyX19yYAG")]
    public void ConvertGbfHelpTweetToRequest_InvalidTextContent_ThrowArgumentException(string text)
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            text);
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        Action act = () => service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid text content");
    }
}
