using System.Linq;
using FluentAssertions;
using GbfRaidFinder.Models;
using GbfRaidFinder.Services;
using Xunit;

namespace GbfRaidFinder.Tests.Services;

public class InMemBossesServiceTests
{
    [Fact]
    public void AddRaidBoss_ItemNotExist_AddedAndReturnTrue()
    {
        // Arrange
        GbfRaidCode code = new("", "");
        GbfRaidBoss boss = new(1, code);

        InMemBossesService service = new();

        // Act
        var isAdded = service.AddRaidBoss(boss);

        // Assert
        isAdded.Should().BeTrue();
        service.Bosses.Count.Should().Be(1);
    }

    [Fact]
    public void AddRaidBoss_ItemExist_NotAddedAndReturnFalse()
    {
        // Arrange
        GbfRaidCode code = new("", "");
        GbfRaidBoss boss = new(1, code);

        InMemBossesService service = new();
        service.AddRaidBoss(boss);

        // Act
        var isAdded = service.AddRaidBoss(boss);

        // Assert
        isAdded.Should().BeFalse();
        service.Bosses.Count.Should().Be(1);
    }


    [Fact]
    public void AddRaidCode_QueueLessThan5_NoDequeue()
    {
        // Arrange
        GbfRaidBoss boss = new(1, new("0", ""));
        InMemBossesService service = new();
        service.AddRaidBoss(boss);
        for (int i = 1; i < 4; i++)
            service.AddRaidCode(boss.PerceptualHash, new(i.ToString(), ""));

        // Act
        service.AddRaidCode(boss.PerceptualHash, new("4", ""));

        // Assert
        var codes = service.Bosses[boss.PerceptualHash].RaidCodes;
        codes.Count.Should().Be(5);
        codes.ElementAt(0).Code.Should().Be("0");
        codes.ElementAt(4).Code.Should().Be("4");
    }

    [Fact]
    public void AddRaidCode_QueueMoreOrEqual5_Dequeue()
    {
        // Arrange
        GbfRaidBoss boss = new(1, new("0", ""));
        InMemBossesService service = new();
        service.AddRaidBoss(boss);
        for (int i = 1; i < 5; i++)
            service.AddRaidCode(boss.PerceptualHash, new(i.ToString(), ""));

        // Act
        service.AddRaidCode(boss.PerceptualHash, new("5", ""));

        // Assert
        var codes = service.Bosses[boss.PerceptualHash].RaidCodes;
        codes.Count.Should().Be(5);
        codes.ElementAt(0).Code.Should().Be("1");
        codes.ElementAt(4).Code.Should().Be("5");
    }
}
