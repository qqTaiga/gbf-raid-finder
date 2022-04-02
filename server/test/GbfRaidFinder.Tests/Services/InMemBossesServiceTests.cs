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
        GbfRaidBoss boss = new("1", code);

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
        GbfRaidBoss boss = new("1", code);

        InMemBossesService service = new();
        service.AddRaidBoss(boss);

        // Act
        var isAdded = service.AddRaidBoss(boss);

        // Assert
        isAdded.Should().BeFalse();
        service.Bosses.Count.Should().Be(1);
    }


    [Fact]
    public void AddRaidCode_QueueLessThan_NoDequeue()
    {
        // Arrange
        GbfRaidBoss boss = new("1", new("0", ""));
        InMemBossesService service = new();
        var max = service.MAXRAIDCODECOUNT;
        service.AddRaidBoss(boss);
        for (int i = 1; i < max - 1; i++)
            service.AddRaidCode(boss.PerceptualHash, new(i.ToString(), ""));

        // Act
        service.AddRaidCode(boss.PerceptualHash, new((max - 1).ToString(), ""));

        // Assert
        var codes = service.Bosses[boss.PerceptualHash].RaidCodes;
        codes.Count.Should().Be(max);
        codes.ElementAt(0).Code.Should().Be("0");
        codes.ElementAt(max - 1).Code.Should().Be((max - 1).ToString());
    }

    [Fact]
    public void AddRaidCode_QueueMoreOrEqual5_Dequeue()
    {
        // Arrange
        GbfRaidBoss boss = new("1", new("0", ""));
        InMemBossesService service = new();
        var max = service.MAXRAIDCODECOUNT;
        service.AddRaidBoss(boss);
        for (int i = 1; i < max; i++)
            service.AddRaidCode(boss.PerceptualHash, new(i.ToString(), ""));

        // Act
        service.AddRaidCode(boss.PerceptualHash, new(max.ToString(), ""));

        // Assert
        var codes = service.Bosses[boss.PerceptualHash].RaidCodes;
        codes.Count.Should().Be(max);
        codes.ElementAt(0).Code.Should().Be("1");
        codes.ElementAt(max - 1).Code.Should().Be(max.ToString());
    }
}
