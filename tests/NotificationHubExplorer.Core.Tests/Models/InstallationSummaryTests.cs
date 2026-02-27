using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Core.Tests.Models;

[TestClass]
public class InstallationSummaryTests
{
    [TestMethod]
    public void InstallationSummary_Should_HoldCorrectValues_When_Constructed()
    {
        var expiration = DateTimeOffset.UtcNow.AddDays(30);

        var summary = new InstallationSummary
        {
            InstallationId = "device-001",
            Platform = "Apns",
            PushChannel = "channel-token-abc",
            ExpirationTime = expiration
        };

        summary.InstallationId.Should().Be("device-001");
        summary.Platform.Should().Be("Apns");
        summary.PushChannel.Should().Be("channel-token-abc");
        summary.ExpirationTime.Should().Be(expiration);
    }

    [TestMethod]
    public void InstallationSummary_Should_AllowNullOptionalFields()
    {
        var summary = new InstallationSummary
        {
            InstallationId = "device-002",
            Platform = "Fcm",
            PushChannel = null,
            ExpirationTime = null
        };

        summary.PushChannel.Should().BeNull();
        summary.ExpirationTime.Should().BeNull();
    }
}
