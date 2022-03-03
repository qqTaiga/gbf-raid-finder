using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using GbfRaidFinder.Models;
using GbfRaidFinder.Services;
using Xunit;

namespace GbfRaidFinder.Tests.Services;

public class GbfRaidServiceTests
{
    [Fact]
    public void GbfHelpTweetToRequest_JapaneseText_GetGbfHelpRequest()
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T16:08:11.000Z",
            "1499416354302021635",
            @"A6806FCC :参戦ID\n参加者募集！\nLv100 ウリエル\nhttps://t.co/GGyX19yYAG");
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
        raidRequest.CreatedAt.Should().Be(3.March(2022).At(16, 08, 11));
        raidRequest.BossName.Should().Be("Lv100 ウリエル");
        raidRequest.RaidCode.Should().Be("A6806FCC");
        raidRequest.ImageUrl.Should().Be("https://pbs.twimg.com/media/C66623wU8AACyL2.jpg");
    }

    [Fact]
    public void GbfHelpTweetToRequest_EnglishText_GetGbfHelpRequest()
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            @"B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b");
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
        raidRequest.CreatedAt.Should().Be(3.March(2022).At(15, 32, 41));
        raidRequest.BossName.Should().Be("Lvl 200 Lindwurm");
        raidRequest.RaidCode.Should().Be("B1154E88");
        raidRequest.ImageUrl.Should().Be("https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
    }

    [Theory]
    [InlineData(null, "value", "value", "value", nameof(GbfHelpTweetData.Created_At))]
    [InlineData("value", null, "value", "value", nameof(GbfHelpTweetData.Text))]
    [InlineData("value", "value", null, "value", nameof(GbfHelpTweetMedia.Type))]
    [InlineData("value", "value", "value", null, nameof(GbfHelpTweetMedia.Url))]
    public void GbfHelpTweetToRequest_MissingRequiredValue_ThrowArgumentException(string createdAt,
        string text,
        string mediaType,
        string mediaUrl,
        string missingValueFieldName)
    {
        // Arrange
        GbfHelpTweetData data = new(createdAt,
            "1499407423215333378",
            text);
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            mediaType,
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
    public void GbfHelpTweetToRequest_MediaCountNotEqualOne_ThrowArgumentException()
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            @"B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b");
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg");
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);
        GbfRaidService service = new();

        // Act
        Action act = () => service.ConvertGbfHelpTweetToRequest(tweet);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Media count is not equal to 1");
    }

    [Fact]
    public void GbfHelpTweetToRequest_InvalidTextContent_ThrowArgumentException()
    {
        // Arrange
        GbfHelpTweetData data = new("2022-03-03T15:32:41.000Z",
            "1499407423215333378",
            @"B1154E88 :Battle ID\nI need backup!\nLvl 200 Lindwurm\nhttps://t.co/VylsLCm88b");
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
