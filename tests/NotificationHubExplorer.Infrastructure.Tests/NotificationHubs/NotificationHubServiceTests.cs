using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationHubExplorer.Core.Interfaces;
using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Infrastructure.Tests.NotificationHubs;

[TestClass]
public class NotificationHubServiceTests
{
    private Mock<INotificationHubService> _mockService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockService = new Mock<INotificationHubService>();
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_ReturnPagedResult_When_SdkReturnsData()
    {
        var items = new List<InstallationSummary>
        {
            new() { InstallationId = "test-id-1", Platform = "Apns" },
            new() { InstallationId = "test-id-2", Platform = "Fcm" }
        };

        _mockService
            .Setup(s => s.GetInstallationsAsync("test-hub", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<InstallationSummary> { Items = items });

        var result = await _mockService.Object.GetInstallationsAsync("test-hub", null, CancellationToken.None);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items[0].InstallationId.Should().Be("test-id-1");
        result.Items[1].InstallationId.Should().Be("test-id-2");
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_HandleContinuationToken_When_MorePagesAvailable()
    {
        var firstPageItems = new List<InstallationSummary>
        {
            new() { InstallationId = "page1-id", Platform = "Apns" }
        };

        _mockService
            .Setup(s => s.GetInstallationsAsync("test-hub", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<InstallationSummary>
            {
                Items = firstPageItems,
                ContinuationToken = "cont-token"
            });

        var result = await _mockService.Object.GetInstallationsAsync("test-hub", null, CancellationToken.None);

        result.ContinuationToken.Should().Be("cont-token");
        result.HasMore.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetInstallationAsync_Should_ReturnInstallationDetails_When_InstallationExists()
    {
        var details = new InstallationDetails
        {
            InstallationId = "device-abc",
            Platform = "Apns",
            PushChannel = "push-token-xyz",
            Tags = new Dictionary<string, string> { ["env"] = "production" }
        };

        _mockService
            .Setup(s => s.GetInstallationAsync("test-hub", "device-abc", It.IsAny<CancellationToken>()))
            .ReturnsAsync(details);

        var result = await _mockService.Object.GetInstallationAsync("test-hub", "device-abc", CancellationToken.None);

        result.InstallationId.Should().Be("device-abc");
        result.Platform.Should().Be("Apns");
        result.PushChannel.Should().Be("push-token-xyz");
        result.Tags.Should().ContainKey("env");
    }

    [TestMethod]
    public async Task DeleteInstallationAsync_Should_ThrowMeaningfulException_When_InstallationNotFound()
    {
        _mockService
            .Setup(s => s.DeleteInstallationAsync("test-hub", "nonexistent", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new KeyNotFoundException("Installation 'nonexistent' was not found."));

        var act = async () => await _mockService.Object.DeleteInstallationAsync(
            "test-hub", "nonexistent", CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*nonexistent*");
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_ThrowMeaningfulException_When_ServiceFails()
    {
        _mockService
            .Setup(s => s.GetInstallationsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Connection to Notification Hub failed."));

        var act = async () => await _mockService.Object.GetInstallationsAsync(
            "test-hub", null, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Connection*");
    }
}
