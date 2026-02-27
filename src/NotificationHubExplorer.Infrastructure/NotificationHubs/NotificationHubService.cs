using Microsoft.Azure.NotificationHubs;
using NotificationHubExplorer.Core.Interfaces;
using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Infrastructure.NotificationHubs;

public sealed class NotificationHubService : INotificationHubService
{
    private readonly NotificationHubClientFactory _factory;

    public NotificationHubService(NotificationHubClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<PagedResult<InstallationSummary>> GetInstallationsAsync(
        string hubName,
        string? continuationToken,
        CancellationToken cancellationToken)
    {
        var client = _factory.GetClient(hubName);

        // The SDK does not support listing all installations directly.
        // Use registrations as a proxy.
        var result = continuationToken is null
            ? await client.GetAllRegistrationsAsync(100, cancellationToken)
            : await client.GetAllRegistrationsAsync(continuationToken, 100, cancellationToken);

        var items = result
            .Select(r => new InstallationSummary
            {
                InstallationId = r.RegistrationId ?? string.Empty,
                Platform = r.GetType().Name.Replace("RegistrationDescription", string.Empty),
                PushChannel = r.PnsHandle,
                ExpirationTime = r.ExpirationTime.HasValue
                    ? new DateTimeOffset(r.ExpirationTime.Value, TimeSpan.Zero)
                    : null
            })
            .ToList();

        return new PagedResult<InstallationSummary>
        {
            Items = items,
            ContinuationToken = result.ContinuationToken
        };
    }

    public async Task<InstallationDetails> GetInstallationAsync(
        string hubName,
        string installationId,
        CancellationToken cancellationToken)
    {
        var client = _factory.GetClient(hubName);
        var installation = await client.GetInstallationAsync(installationId, cancellationToken);

        return new InstallationDetails
        {
            InstallationId = installation.InstallationId,
            Platform = installation.Platform.ToString(),
            PushChannel = installation.PushChannel,
            ExpirationTime = installation.ExpirationTime,
            // The SDK exposes tags as IList<string> where each entry is the full tag string
            // (e.g., "userId:123"). Map each tag to itself so callers receive a consistent
            // dictionary; callers may parse the values further as needed.
            Tags = installation.Tags?.ToDictionary(t => t, t => t) ?? new Dictionary<string, string>(),
            Templates = installation.Templates?.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Body ?? string.Empty) ?? new Dictionary<string, string>()
        };
    }

    public async Task DeleteInstallationAsync(
        string hubName,
        string installationId,
        CancellationToken cancellationToken)
    {
        var client = _factory.GetClient(hubName);
        await client.DeleteInstallationAsync(installationId, cancellationToken);
    }
}
