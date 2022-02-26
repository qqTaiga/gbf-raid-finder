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

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

        // Act
        var result = await twitterFilteredStreamService.ModifyRulesAsync(
            TwitterFilteredStreamRuleActions.Add,
            true,
            new TwitterFilteredStreamRule[] { new("a") });

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

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

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
            "{\"data\":{\"attachments\":{\"media_keys\":[\"3_707901161920028674\"]},\"created_at\":\"2022-02-25T16:48:52.000Z\",\"id\":\"1497252266176577543\",\"text\":\"4/30 10F8A733 :参戦ID\\n参加者募集！\\nLv150 プロトバハムート\\nhttps://t.co/C53UwBpmJ8\"},\"includes\":{\"media\":[{\"media_key\":\"3_707901161920028674\",\"type\":\"photo\"}]},\"matching_rules\":[{\"id\":\"1496876690072825856\",\"tag\":\"gbf help twitter post\"}]}" +
            "\n" +
            "{\"data\":{\"attachments\":{\"media_keys\":[\"3_707901161920028674\"]},\"created_at\":\"2022-02-25T16:48:52.000Z\",\"id\":\"1497252266176577543\",\"text\":\"4/30 10F8A733 :参戦ID\\n参加者募集！\\nLv150 プロトバハムート\\nhttps://t.co/C53UwBpmJ8\"},\"includes\":{\"media\":[{\"media_key\":\"3_707901161920028674\",\"type\":\"photo\"}]},\"matching_rules\":[{\"id\":\"1496876690072825856\",\"tag\":\"gbf help twitter post\"}]}" +
            "\n" +
            "{\"data\":{\"attachments\":{\"media_keys\":[\"3_707901161920028674\"]},\"created_at\":\"2022-02-25T16:48:52.000Z\",\"id\":\"1497252266176577543\",\"text\":\"4/30 10F8A733 :参戦ID\\n参加者募集！\\nLv150 プロトバハムート\\nhttps://t.co/C53UwBpmJ8\"},\"includes\":{\"media\":[{\"media_key\":\"3_707901161920028674\",\"type\":\"photo\"}]},\"matching_rules\":[{\"id\":\"1496876690072825856\",\"tag\":\"gbf help twitter post\"}]}";
        var httpClient = MockUtils.MockHttpClient(HttpStatusCode.OK, new StringContent(returnText));
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

        // Act
        var result = twitterFilteredStreamService.ConnectStreamAsync();

        // Assert
        await foreach (var tweet in result)
        {
            tweet.Should().NotBeNull();
            tweet?.Text.Should().NotBeNull();
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

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

        // Act
        Func<Task> action = async ()
            => await twitterFilteredStreamService.ConnectStreamAsync().FirstOrDefaultAsync();

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>()
            .WithMessage(statusCode.ToString());
    }
}
