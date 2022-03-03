// using FluentAssertions;
// using FluentAssertions.Extensions;
// using GbfRaidFinder.Models;
// using GbfRaidFinder.Services;
// using Xunit;
//
// namespace GbfRaidFinder.Tests.Services;
//
// public class GbfRaidServiceTests
// {
//     [Fact]
//     public void GbfHelpTweetToRequest_JapaneseText_GetGbfHelpRequest()
//     {
//         // Arrange
//         GbfHelpTweetAttachment attachment = new(new[] { "3_707901161920028674" });
//         GbfHelpTweet tweet = new(attachment,
//             "2022-02-25T16:48:52.000Z",
//             "1497252266176577543",
//             "4/30 10F8A733 :参戦ID\\n参加者募集！\\nLv150 プロトバハムート\\nhttps://t.co/C53UwBpmJ8");
//         GbfRaidService service = new();
//         // Act
//         var raidRequest = service.ConvertGbfHelpTweetToRequest(tweet);
//
//         // Assert
//         raidRequest.Should().NotBeNull();
//         raidRequest.CreatedAt.Should().Be(25.February(2022).At(16, 48, 52));
//         raidRequest.BossName.Should().Be("Lv150 プロトバハムート");
//         raidRequest.RaidCode.Should().Be("10F8A733");
//         raidRequest.ImageId.Should().Be("3_707901161920028674");
//     }
//
//     [Fact]
//     public void GbfHelpTweetToRequest_EnglishText_GetGbfHelpRequest()
//     {
//         // Arrange
//         GbfHelpTweetAttachment attachment = new(new[] { "3_707901161920028674" });
//         GbfHelpTweet tweet = new(attachment,
//             "2022-02-25T21:22:52.000Z",
//             "1497252266176577543",
//             "4/30 10F8A733 :参戦ID\\n参加者募集！\\nLv150 プロトバハムート\\nhttps://t.co/C53UwBpmJ8");
//         GbfRaidService service = new();
//         // Act
//         var raidRequest = service.ConvertGbfHelpTweetToRequest(tweet);
//
//         // Assert
//         raidRequest.Should().NotBeNull();
//         raidRequest.CreatedAt.Should().Be(25.February(2022).At(16, 48, 52));
//         raidRequest.BossName.Should().Be("Lv150 プロトバハムート");
//         raidRequest.RaidCode.Should().Be("10F8A733");
//         raidRequest.ImageId.Should().Be("3_707901161920028674");
//     }
// }
