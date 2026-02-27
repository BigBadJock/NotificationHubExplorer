using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Core.Interfaces;

public interface INotificationHubService
{
    Task<PagedResult<InstallationSummary>> GetInstallationsAsync(
        string hubName,
        string? continuationToken,
        CancellationToken cancellationToken);

    Task<InstallationDetails> GetInstallationAsync(
        string hubName,
        string installationId,
        CancellationToken cancellationToken);

    Task DeleteInstallationAsync(
        string hubName,
        string installationId,
        CancellationToken cancellationToken);
}
