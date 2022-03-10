using System.Threading.Tasks;
using FluentAssertions;
using GbfRaidFinder.Controllers;
using GbfRaidFinder.Models;
using GbfRaidFinder.Models.Dtos;
using GbfRaidFinder.Models.Enums;
using GbfRaidFinder.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GbfRaidFinder.Tests.Controllers;

public class TwitterControllerTests
{
    [Fact]
    public async Task ModifyRules_Success_Return200()
    {
        // Arrange
        TwitterFilteredStreamRuleDto input = new(
            TwitterFilteredStreamRuleActions.Add,
            true,
            new TwitterFilteredStreamRule[] { new("test") },
            new string[] { }
        );
        Mock<ITwitterFilteredStreamService> twitterFSServiceMock = new();
        twitterFSServiceMock.Setup(_ => _.ModifyRulesAsync(
            It.IsAny<TwitterFilteredStreamRuleActions>(),
            It.IsAny<bool>(),
            It.IsAny<TwitterFilteredStreamRule[]>(),
            It.IsAny<string[]>()))
                .ReturnsAsync(new HttpResult(true));
        TwitterController controller = new(twitterFSServiceMock.Object);

        // Act
        var result = (OkObjectResult)await controller.ModifyRules(input);

        // Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ModifyRules_Failed_Return422()
    {
        // Arrange
        TwitterFilteredStreamRuleDto input = new(
            TwitterFilteredStreamRuleActions.Add,
            true,
            new TwitterFilteredStreamRule[] { new("test") },
            new string[] { }
        );
        Mock<ITwitterFilteredStreamService> twitterFSServiceMock = new();
        twitterFSServiceMock.Setup(_ => _.ModifyRulesAsync(
            It.IsAny<TwitterFilteredStreamRuleActions>(),
            It.IsAny<bool>(),
            It.IsAny<TwitterFilteredStreamRule[]>(),
            It.IsAny<string[]>()))
                .ReturnsAsync(new HttpResult(false) { ErrorDesc = new { Error = "Error" } });
        TwitterController controller = new(twitterFSServiceMock.Object);

        // Act
        var result = (UnprocessableEntityObjectResult)await controller.ModifyRules(input);

        // Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(422);
    }

    [Fact]
    public async Task RetrieveRules_Success_Return200WithContent()
    {
        // Arrange
        Mock<ITwitterFilteredStreamService> twitterFSServiceMock = new();
        twitterFSServiceMock.Setup(_ => _.RetrieveRulesAsync())
                .ReturnsAsync(new HttpResult(true) { Content = new { Content = "Test" } });
        TwitterController controller = new(twitterFSServiceMock.Object);

        // Act
        var result = (OkObjectResult)await controller.RetrieveRules();

        // Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(200);
        result?.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task RetrieveRules_Failed_Return400WithErrorDesc()
    {
        // Arrange
        Mock<ITwitterFilteredStreamService> twitterFSServiceMock = new();
        twitterFSServiceMock.Setup(_ => _.RetrieveRulesAsync())
                .ReturnsAsync(new HttpResult(false) { ErrorDesc = new { Error = "Error" } });
        TwitterController controller = new(twitterFSServiceMock.Object);

        // Act
        var result = (BadRequestObjectResult)await controller.RetrieveRules();

        // Assert
        result.Should().NotBeNull();
        result?.StatusCode.Should().Be(400);
        result?.Value.Should().NotBeNull();
    }
}

