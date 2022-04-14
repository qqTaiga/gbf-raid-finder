using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using GbfRaidFinder.BackgroundTasks;
using GbfRaidFinder.Hubs;
using GbfRaidFinder.Models;
using GbfRaidFinder.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace GbfRaidFinder.Tests.BackgroundTasks;

public class GbfConnectStreamTaskTests
{
    private GbfConnectStreamTask Init(IGbfRaidService? gbfRaidService
        , IInMemBossesService? inMemService)
    {
        var log = new NullLogger<GbfConnectStreamTask>();

        GbfMapperService mapperService = new();

        var _gbfRaidService = gbfRaidService != null
            ? gbfRaidService
            : new Mock<IGbfRaidService>().Object;

        Mock<IHubContext<GbfRaidHub, IGbfRaidHub>> gbfRaidHubMock = new();
        gbfRaidHubMock.Setup(_ => _.Clients.Group(It.IsAny<string>())
            .ReceiveRaidCode(It.IsAny<string>(), It.IsAny<GbfRaidCode>()));
        var _gbfRaidHub = gbfRaidHubMock.Object;

        var _inMemService = inMemService != null
            ? inMemService
            : new Mock<IInMemBossesService>().Object;

        var _streamService = new Mock<ITwitterFilteredStreamService>().Object;

        return new GbfConnectStreamTask(log,
            mapperService,
            _gbfRaidService,
            _gbfRaidHub,
            _inMemService,
            _streamService);
    }

    [Fact]
    public async Task RunMainTask_NewBoss_AddNewBoss()
    {
        // Arrange
        var createdAt = "2022-03-03T15:32:41.000Z";
        var bossName = "Lv100 ウリエル";
        var code = "A6806FCC";
        var level = 100;
        var imageUrl = "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg";
        GbfHelpTweetData data = new(createdAt,
            "1499407423215333378",
            $"{code} :参戦ID\n参加者募集！\n{bossName}\nhttps://t.co/GGyX19yYAG");
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            imageUrl);
        GbfHelpTweetExpansion expansion = new(new[] { media });
        GbfHelpTweet tweet = new(data, expansion);

        GbfHelpRequest req = new(Language.Japanese, createdAt, bossName, level, code, imageUrl);

        string perceptualhash = "1";
        Mock<IGbfRaidService> gbfRaidService = new();
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet)).Returns(req);
        gbfRaidService.Setup(_ =>
            _.GetImagePerceptualHashAsync(It.IsAny<string>())).ReturnsAsync(perceptualhash);

        InMemBossesService inMemService = new();

        var currentTime = DateTime.Now;
        var task = Init(gbfRaidService.Object, inMemService);

        // Act
        await task.RunMainTask(tweet);

        // Assert
        inMemService.Bosses.Count.Should().Be(1);
        var boss = inMemService.Bosses[perceptualhash];
        boss.PerceptualHash.Should().Be(perceptualhash);
        boss.LastEncounterAt.Should().BeAfter(currentTime);
        boss.EngName.Should().BeNull();
        boss.JapName.Should().Be(bossName);
        boss.RaidCodes.Count.Should().Be(1);
    }

    [Fact]
    public async Task RunMainTask_ExistingBoss_AddNewCode()
    {
        // Arrange
        var bossName = "Lv100 ウリエル";
        var level = 100;
        var imageUrl = "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg";
        GbfHelpTweetMedia media = new("3_1236847343103705088",
            "photo",
            imageUrl);
        GbfHelpTweetExpansion expansion = new(new[] { media });

        var createdAt1 = "2022-03-03T15:32:41.000Z";
        var code1 = "A6806FCC";
        GbfHelpTweetData data1 = new(createdAt1,
            "1499407423215333378",
            $"{code1} :参戦ID\n参加者募集！\n{bossName}\nhttps://t.co/GGyX19yYAG");
        GbfHelpTweet tweet1 = new(data1, expansion);
        GbfHelpRequest req1 = new(Language.Japanese, createdAt1, bossName, level, code1, imageUrl);

        var createdAt2 = "2022-03-03T15:33:41.000Z";
        var code2 = "AB381GEE";
        GbfHelpTweetData data2 = new(createdAt2,
            "1499407423215333378",
            $"{code2} :参戦ID\n参加者募集！\n{bossName}\nhttps://t.co/GGyX19yYAG");
        GbfHelpTweet tweet2 = new(data2, expansion);
        GbfHelpRequest req2 = new(Language.Japanese, createdAt2, bossName, level, code2, imageUrl);

        string perceptualhash = "1";
        Mock<IGbfRaidService> gbfRaidService = new();
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet1)).Returns(req1);
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet2)).Returns(req2);
        gbfRaidService.Setup(_ =>
            _.GetImagePerceptualHashAsync(It.IsAny<string>())).ReturnsAsync(perceptualhash);

        InMemBossesService inMemService = new();

        var task = Init(gbfRaidService.Object, inMemService);

        // Act
        await task.RunMainTask(tweet1);
        var currentTime = DateTime.Now;
        await task.RunMainTask(tweet2);

        // Assert
        inMemService.Bosses.Count.Should().Be(1);
        var boss = inMemService.Bosses[perceptualhash];
        boss.PerceptualHash.Should().Be(perceptualhash);
        boss.LastEncounterAt.Should().BeAfter(currentTime);
        boss.EngName.Should().BeNull();
        boss.JapName.Should().Be(bossName);
        boss.RaidCodes.Count.Should().Be(2);

        var raidCode1 = boss.RaidCodes.ElementAt(0);
        raidCode1.Code.Should().Be(code1);
        raidCode1.CreatedAt.Should().Be(createdAt1);

        var raidCode2 = boss.RaidCodes.ElementAt(1);
        raidCode2.Code.Should().Be(code2);
        raidCode2.CreatedAt.Should().Be(createdAt2);
    }

    [Fact]
    public async Task RunMainTask_ExistingBossWithDifferentLang_AddNameAndCode()
    {
        // Arrange
        var createdAt1 = "2022-03-03T15:32:41.000Z";
        var bossName1 = "Lv100 ウリエル";
        var level1 = 100;
        var code1 = "A6806FCC";
        var imageUrl1 = "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg";
        GbfHelpTweetData data1 = new(createdAt1,
            "1499407423215333378",
            $"{code1} :参戦ID\n参加者募集！\n{bossName1}\nhttps://t.co/GGyX19yYAG");
        GbfHelpTweetMedia media1 = new("3_1236847343103705088",
            "photo",
            imageUrl1);
        GbfHelpTweetExpansion expansion1 = new(new[] { media1 });
        GbfHelpTweet tweet1 = new(data1, expansion1);
        GbfHelpRequest req1 = new(Language.Japanese, createdAt1, bossName1, level1, code1, imageUrl1);

        var createdAt2 = "2022-03-03T15:32:55.000Z";
        var bossName2 = "Lvl 100 Uriel";
        var level2 = 100;
        var code2 = "AB3D141E";
        var imageUrl2 = "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg";
        GbfHelpTweetData data2 = new(createdAt1,
            "1499407423215333378",
            $"{code2} :Battle ID\nI need backup!\n{bossName2}");
        GbfHelpTweetMedia media2 = new("3_1236847343103705088",
            "photo",
            imageUrl2);
        GbfHelpTweetExpansion expansion2 = new(new[] { media2 });
        GbfHelpTweet tweet2 = new(data2, expansion2);
        GbfHelpRequest req2 = new(Language.English, createdAt2, bossName2, level2, code2, imageUrl1);

        string perceptualhash = "1";
        Mock<IGbfRaidService> gbfRaidService = new();
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet1)).Returns(req1);
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet2)).Returns(req2);
        gbfRaidService.Setup(_ =>
            _.GetImagePerceptualHashAsync(It.IsAny<string>())).ReturnsAsync(perceptualhash);

        InMemBossesService inMemService = new();

        var task = Init(gbfRaidService.Object, inMemService);

        // Act
        await task.RunMainTask(tweet1);
        var currentTime = DateTime.Now;
        await task.RunMainTask(tweet2);

        // Assert
        inMemService.Bosses.Count.Should().Be(1);
        var boss = inMemService.Bosses[perceptualhash];
        boss.PerceptualHash.Should().Be(perceptualhash);
        boss.LastEncounterAt.Should().BeAfter(currentTime);
        boss.JapName.Should().Be(bossName1);
        boss.EngName.Should().Be(bossName2);
        boss.RaidCodes.Count.Should().Be(2);

        var raidCode1 = boss.RaidCodes.ElementAt(0);
        raidCode1.Code.Should().Be(code1);
        raidCode1.CreatedAt.Should().Be(createdAt1);

        var raidCode2 = boss.RaidCodes.ElementAt(1);
        raidCode2.Code.Should().Be(code2);
        raidCode2.CreatedAt.Should().Be(createdAt2);
    }

    [Fact]
    public async Task RunMainTask_ExistingBossWithDifferentLangNeedMap_AddNameAndCode()
    {
        // Arrange
        var createdAt1 = "2022-03-03T15:32:41.000Z";
        var bossName1 = "Lv120 メドゥーサ";
        var level1 = 120;
        var code1 = "A6806FCC";
        var imageUrl1 = "https://pbs.twimg.com/media/ESoqGv8VAAA2OzG.jpg";
        GbfHelpTweetData data1 = new(createdAt1,
            "1499407423215333378",
            $"{code1} :参戦ID\n参加者募集！\n{bossName1}\nhttps://t.co/GGyX19yYAG");
        GbfHelpTweetMedia media1 = new("3_1236847343103705088",
            "photo",
            imageUrl1);
        GbfHelpTweetExpansion expansion1 = new(new[] { media1 });
        GbfHelpTweet tweet1 = new(data1, expansion1);
        GbfHelpRequest req1 = new(Language.Japanese, createdAt1, bossName1, level1, code1, imageUrl1);

        var createdAt2 = "2022-03-03T15:32:55.000Z";
        var bossName2 = "Lvl 120 Medusa";
        var level2 = 120;
        var code2 = "AB3D141E";
        var imageUrl2 = "https://pbs.twimg.com/media/CYBki-CUkAQVWW_?format=jpg&name=medium";
        GbfHelpTweetData data2 = new(createdAt1,
            "1499407423215333378",
            $"{code2} :Battle ID\nI need backup!\n{bossName2}");
        GbfHelpTweetMedia media2 = new("3_1236847343103705088",
            "photo",
            imageUrl2);
        GbfHelpTweetExpansion expansion2 = new(new[] { media2 });
        GbfHelpTweet tweet2 = new(data2, expansion2);
        GbfHelpRequest req2 = new(Language.English, createdAt2, bossName2, level2, code2, imageUrl2);

        string perceptualhash = "17682549972253862360";
        Mock<IGbfRaidService> gbfRaidService = new();
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet1)).Returns(req1);
        gbfRaidService.Setup(_ =>
            _.ConvertGbfHelpTweetToRequest(tweet2)).Returns(req2);
        gbfRaidService.Setup(_ =>
            _.GetImagePerceptualHashAsync(imageUrl1)).ReturnsAsync(perceptualhash);
        gbfRaidService.Setup(_ =>
            _.GetImagePerceptualHashAsync(It.IsNotIn<string>(imageUrl1))).ReturnsAsync("1");

        InMemBossesService inMemService = new();

        var task = Init(gbfRaidService.Object, inMemService);

        // Act
        await task.RunMainTask(tweet1);
        var currentTime = DateTime.Now;
        await task.RunMainTask(tweet2);

        // Assert
        inMemService.Bosses.Count.Should().Be(1);
        var boss = inMemService.Bosses[perceptualhash];
        boss.PerceptualHash.Should().Be(perceptualhash);
        boss.LastEncounterAt.Should().BeAfter(currentTime);
        boss.JapName.Should().Be(bossName1);
        boss.EngName.Should().Be(bossName2);
        boss.RaidCodes.Count.Should().Be(2);

        var raidCode1 = boss.RaidCodes.ElementAt(0);
        raidCode1.Code.Should().Be(code1);
        raidCode1.CreatedAt.Should().Be(createdAt1);

        var raidCode2 = boss.RaidCodes.ElementAt(1);
        raidCode2.Code.Should().Be(code2);
        raidCode2.CreatedAt.Should().Be(createdAt2);
    }
}
