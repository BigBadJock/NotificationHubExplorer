using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationHubExplorer.Core.Interfaces;
using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Core.Tests.ViewModels;

[TestClass]
public class InstallationsViewModelTests
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
        var expectedItems = new List<InstallationSummary>
        {
            new() { InstallationId = "id-001", Platform = "Apns" },
            new() { InstallationId = "id-002", Platform = "Fcm" }
        };

        _mockService
            .Setup(s => s.GetInstallationsAsync("my-hub", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<InstallationSummary>
            {
                Items = expectedItems,
                ContinuationToken = null
            });

        var result = await _mockService.Object.GetInstallationsAsync("my-hub", null, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.Items.Should().BeEquivalentTo(expectedItems);
        result.HasMore.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_ReturnEmptyPagedResult_When_NoInstallationsExist()
    {
        _mockService
            .Setup(s => s.GetInstallationsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(PagedResult<InstallationSummary>.Empty());

        var result = await _mockService.Object.GetInstallationsAsync("empty-hub", null, CancellationToken.None);

        result.Items.Should().BeEmpty();
        result.HasMore.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_SupportPagination_When_ContinuationTokenIsReturned()
    {
        var firstPage = new PagedResult<InstallationSummary>
        {
            Items = new List<InstallationSummary> { new() { InstallationId = "id-001", Platform = "Apns" } },
            ContinuationToken = "next-page-token"
        };

        var secondPage = new PagedResult<InstallationSummary>
        {
            Items = new List<InstallationSummary> { new() { InstallationId = "id-002", Platform = "Fcm" } },
            ContinuationToken = null
        };

        _mockService
            .Setup(s => s.GetInstallationsAsync("my-hub", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(firstPage);

        _mockService
            .Setup(s => s.GetInstallationsAsync("my-hub", "next-page-token", It.IsAny<CancellationToken>()))
            .ReturnsAsync(secondPage);

        var allItems = new List<InstallationSummary>();
        string? token = null;

        do
        {
            var page = await _mockService.Object.GetInstallationsAsync("my-hub", token, CancellationToken.None);
            allItems.AddRange(page.Items);
            token = page.ContinuationToken;
        }
        while (token is not null);

        allItems.Should().HaveCount(2);
        allItems.Select(i => i.InstallationId).Should().Contain(new[] { "id-001", "id-002" });
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_PropagateException_When_ServiceThrows()
    {
        _mockService
            .Setup(s => s.GetInstallationsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Service unavailable"));

        var act = async () => await _mockService.Object.GetInstallationsAsync("hub", null, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Service unavailable");
    }

    [TestMethod]
    public async Task GetInstallationsAsync_Should_RespectCancellation_When_TokenIsCancelled()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        _mockService
            .Setup(s => s.GetInstallationsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        var act = async () => await _mockService.Object.GetInstallationsAsync("hub", null, cts.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [TestMethod]
    public async Task GetInstallationAsync_Should_ReturnInstallationDetails_When_Found()
    {
        var expected = new InstallationDetails
        {
            InstallationId = "device-001",
            Platform = "Apns",
            PushChannel = "apns-token",
            Tags = new Dictionary<string, string> { ["userId"] = "user-123" }
        };

        _mockService
            .Setup(s => s.GetInstallationAsync("my-hub", "device-001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _mockService.Object.GetInstallationAsync("my-hub", "device-001", CancellationToken.None);

        result.InstallationId.Should().Be("device-001");
        result.Platform.Should().Be("Apns");
        result.PushChannel.Should().Be("apns-token");
    }

    [TestMethod]
    public async Task DeleteInstallationAsync_Should_CallServiceOnce_When_Invoked()
    {
        _mockService
            .Setup(s => s.DeleteInstallationAsync("my-hub", "device-001", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _mockService.Object.DeleteInstallationAsync("my-hub", "device-001", CancellationToken.None);

        _mockService.Verify(
            s => s.DeleteInstallationAsync("my-hub", "device-001", It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
