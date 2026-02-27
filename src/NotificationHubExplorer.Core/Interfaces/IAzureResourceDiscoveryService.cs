using NotificationHubExplorer.Core.Models;

namespace NotificationHubExplorer.Core.Interfaces;

public interface IAzureResourceDiscoveryService
{
    Task<IEnumerable<SubscriptionInfo>> GetSubscriptionsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<NamespaceInfo>> GetNamespacesAsync(string subscriptionId, CancellationToken cancellationToken);
    Task<IEnumerable<HubInfo>> GetHubsAsync(string subscriptionId, string namespaceName, CancellationToken cancellationToken);
}
