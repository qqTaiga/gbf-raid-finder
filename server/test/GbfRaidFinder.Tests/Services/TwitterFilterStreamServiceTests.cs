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
    public async Task ModifyRules_NewValidRule_ReturnValid()
    {
        // Arrange
        var httpClient = MockUtils.MockHttpClient("{\"a\": \"a\"}", HttpStatusCode.OK);
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

        // Act
        var result = await twitterFilteredStreamService.ModifyRules(
            TwitterFilteredStreamRuleActions.Add,
            true,
            new TwitterFilteredStreamRule[] { new("a") });

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RetreiveRules_NewValidRule_Return200()
    {
        // Arrange
        var httpClient = MockUtils.MockHttpClient("{\"a\": \"a\"}", HttpStatusCode.OK);
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRule = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

        // Act
        var result = await twitterFilteredStreamService.RetrieveRules();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

}
