using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using GbfRaidFinder.Models.Enums;
using GbfRaidFinder.Models.Settings;
using GbfRaidFinder.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GbfRaidFinder.Tests.Services;

public class TwitterFilteredStreamServiceTests
{
    [Fact]
    public async Task AddRule_NewValidRule_ReturnValid()
    {
        // Arrange
        HttpResponseMessage response = new()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"a\": \"a\"}")
        };
        Mock<HttpClient> httpClient = new();
        httpClient.Setup(_ => _.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(response);
        Mock<IHttpClientFactory> httpClientFactory = new();
        httpClientFactory.Setup(f => f.CreateClient()).Returns(httpClient.Object);

        var keysOption = Options.Create(new Keys { TwitterJwtToken = "test" });
        var urlsOption = Options.Create(new Urls { TwitterFilteredStreamRules = "test" });

        TwitterFilteredStreamService twitterFilteredStreamService = new(
            httpClientFactory.Object, keysOption, urlsOption);

        // Act
        var result = await twitterFilteredStreamService.AddRule(
            TwitterFilteredStreamRuleActions.Add, true, "");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorDesc.Should().BeNull();
    }
}
