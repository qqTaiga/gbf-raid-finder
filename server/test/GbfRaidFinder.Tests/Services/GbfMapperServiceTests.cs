using FluentAssertions;
using GbfRaidFinder.Services;
using Xunit;

namespace GbfRaidFinder.Tests.Services;

public class GbfMapperServiceTests
{
    [Fact]
    public void TryMapToJapPerceptualHash_HasKey_ReturnMappedHash()
    {
        // Arrange
        string bossEngName = "Lvl 120 Medusa";
        Language lang = Language.English;
        GbfMapperService service = new();

        // Act
        var perceptualHash = service.TryMapToJapPerceptualHash(bossEngName, lang);

        // Assert
        perceptualHash.Should().Be("17682549972253862360");
    }

    [Fact]
    public void TryMapToJapPerceptualHash_NoKey_Return0()
    {
        // Arrange
        string bossEngName = "Lvl 150 Proto Bahamut";
        Language lang = Language.English;
        GbfMapperService service = new();

        // Act
        var perceptualHash = service.TryMapToJapPerceptualHash(bossEngName, lang);

        // Assert
        perceptualHash.Should().Be("");
    }

    [Fact]
    public void TryMapToJapPerceptualHash_JapLang_Return0()
    {
        // Arrange
        string bossEngName = "Lvl 120 Medusa";
        Language lang = Language.Japanese;
        GbfMapperService service = new();

        // Act
        var perceptualHash = service.TryMapToJapPerceptualHash(bossEngName, lang);

        // Assert
        perceptualHash.Should().Be("");
    }
}
