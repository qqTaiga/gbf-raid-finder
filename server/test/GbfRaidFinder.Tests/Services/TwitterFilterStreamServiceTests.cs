using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GbfRaidFinder.Models;
using GbfRaidFinder.Models.Enums;
using GbfRaidFinder.Models.Settings;
using GbfRaidFinder.Services;
using GbfRaidFinder.Tests.Utils;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GbfRaidFinder.Tests.Services;

public class TwitterFilteredStreamServiceTests
{
    [Fact]
    public async Task ModifyRulesAsync_NewValidRule_ReturnValid()
    {
        // Arrange
        var httpClient = MockUtils.MockHttpClient(
                HttpStatusCode.OK,
                new StringContent("{\"a\": \"a\"}"));
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var log = new NullLogger<TwitterFilteredStreamService>();

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, log, keysOption, urlsOption);

        // Act
        var result = await twitterFilteredStreamService.ModifyRulesAsync(
            TwitterFilteredStreamRuleActions.Add,
            true,
            new TwitterFilteredStreamRule[] { new("a") },
            new string[] { });

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RetreiveRulesAsync_NewValidRule_Return200()
    {
        // Arrange
        var httpClient = MockUtils.MockHttpClient(
                HttpStatusCode.OK,
                new StringContent("{\"a\": \"a\"}"));
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var log = new NullLogger<TwitterFilteredStreamService>();

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, log, keysOption, urlsOption);

        // Act
        var result = await twitterFilteredStreamService.RetrieveRulesAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ConnectFilteredStreamAsync_ReceiveData_GbfHelpTweetHasValue()
    {
        // Arrange
        string returnText =
            "{\"data\":{\"attachments\":{\"media_keys\":[\"3_841815632207212544\"]},\"created_at\":\"2022-03-03T16:08:11.000Z\",\"id\":\"1499416354302021635\",\"text\":\"A6806FCC :参戦ID\\n参加者募集！\\nLv100 ウリエル\\nhttps://t.co/GGyX19yYAG\"},\"includes\":{\"media\":[{\"media_key\":\"3_841815632207212544\",\"type\":\"photo\",\"url\":\"https://pbs.twimg.com/media/C66623wU8AACyL2.jpg\"}]},\"matching_rules\":[{\"id\":\"1499410123726340098\",\"tag\":\"gbf raid\"}]}" +
            "\n" +
            "{\"data\":{\"attachments\":{\"media_keys\":[\"3_841815632207212544\"]},\"created_at\":\"2022-03-03T16:08:11.000Z\",\"id\":\"1499416354302021635\",\"text\":\"A6806FCC :参戦ID\\n参加者募集！\\nLv100 ウリエル\\nhttps://t.co/GGyX19yYAG\"},\"includes\":{\"media\":[{\"media_key\":\"3_841815632207212544\",\"type\":\"photo\",\"url\":\"https://pbs.twimg.com/media/C66623wU8AACyL2.jpg\"}]},\"matching_rules\":[{\"id\":\"1499410123726340098\",\"tag\":\"gbf raid\"}]}" +
            "\n" +
            "{\"data\":{\"attachments\":{\"media_keys\":[\"3_841815632207212544\"]},\"created_at\":\"2022-03-03T16:08:11.000Z\",\"id\":\"1499416354302021635\",\"text\":\"A6806FCC :参戦ID\\n参加者募集！\\nLv100 ウリエル\\nhttps://t.co/GGyX19yYAG\"},\"includes\":{\"media\":[{\"media_key\":\"3_841815632207212544\",\"type\":\"photo\",\"url\":\"https://pbs.twimg.com/media/C66623wU8AACyL2.jpg\"}]},\"matching_rules\":[{\"id\":\"1499410123726340098\",\"tag\":\"gbf raid\"}]}";
        var httpClient = MockUtils.MockHttpClient(HttpStatusCode.OK, new StringContent(returnText));
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var log = new NullLogger<TwitterFilteredStreamService>();

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, log, keysOption, urlsOption);

        // Act
        var result = twitterFilteredStreamService.ConnectStreamAsync();

        // Assert
        var list = await result.ToListAsync();
        list.Should().NotBeEmpty();
        list.Count.Should().Be(3);
        foreach (var tweet in list)
        {
            tweet.Should().NotBeNull();
            tweet.Data.Created_At.Should().Be("2022-03-03T16:08:11.000Z");
            tweet.Data.Id.Should().Be("1499416354302021635");
            tweet.Data.Text.Should().Be(
                "A6806FCC :参戦ID\n参加者募集！\nLv100 ウリエル\nhttps://t.co/GGyX19yYAG");

            tweet?.Includes.Media.Length.Should().Be(1);
            tweet?.Includes.Media[0]?.Media_Key.Should().Be("3_841815632207212544");
            tweet?.Includes.Media[0]?.Type.Should().Be("photo");
            tweet?.Includes.Media[0]?.Url.Should().Be(
                "https://pbs.twimg.com/media/C66623wU8AACyL2.jpg");
        }
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    public async Task ConnectFilteredStreamAsync_ReceiveError_ThrowException(
        HttpStatusCode statusCode)
    {
        // Arrange
        string returnText = "{\"error\": \"error\"}";
        var httpClient = MockUtils.MockHttpClient(statusCode, new StringContent(returnText));
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var log = new NullLogger<TwitterFilteredStreamService>();

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, log, keysOption, urlsOption);

        // Act
        Func<Task> action = async ()
            => await twitterFilteredStreamService.ConnectStreamAsync().FirstOrDefaultAsync();

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage(statusCode.ToString());
    }
}
